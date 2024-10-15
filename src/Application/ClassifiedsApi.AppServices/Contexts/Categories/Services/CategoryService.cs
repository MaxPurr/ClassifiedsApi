using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Services;
using ClassifiedsApi.AppServices.Contexts.Categories.Builders;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.AppServices.Contexts.Categories.Validators;
using ClassifiedsApi.AppServices.Exceptions.Categories;
using ClassifiedsApi.Contracts.Contexts.Categories;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Services;

/// <inheritdoc />
public class CategoryService : ICategoryService
{
    private const string CacheLabel = "category";
    private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(5);
    
    private readonly ICategoryRepository _repository;
    private readonly ISerializableCache _cache;
    private readonly ICategorySpecificationBuilder _specificationBuilder;
    private readonly ICategoryValidator _categoryValidator;
    
    private readonly ILogger<CategoryService> _logger;
    private readonly IStructuralLoggingService _logService;
    
    private readonly IValidator<CategoryCreate> _categoryCreateValidator;
    private readonly IValidator<CategoryUpdate> _categoryUpdateValidator;
    private readonly IValidator<CategoriesSearch> _categoriesSearchValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий файлов <see cref="ICategoryRepository"/>.</param>
    /// <param name="cache">Сериализуемый кэш <see cref="ISerializableCache"/>.</param>
    /// <param name="specificationBuilder">Строитель спецификаций для категорий <see cref="ICategorySpecificationBuilder"/>.</param>
    /// <param name="categoryCreateValidator">Валидатор модели создания категории.</param>
    /// <param name="categoryUpdateValidator">Валидатор модели обновления категории.</param>
    /// <param name="categoriesSearchValidator">Валидатор модели поиска категорий.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="logService">Сервис структурного логирования.</param>
    /// <param name="categoryValidator">Валидатор категорий.</param>
    public CategoryService(
        ICategoryRepository repository, 
        ISerializableCache cache,
        ICategorySpecificationBuilder specificationBuilder,
        ICategoryValidator categoryValidator,
        ILogger<CategoryService> logger, 
        IStructuralLoggingService logService,
        IValidator<CategoryCreate> categoryCreateValidator, 
        IValidator<CategoryUpdate> categoryUpdateValidator, 
        IValidator<CategoriesSearch> categoriesSearchValidator)
    {
        _repository = repository;
        _cache = cache;
        _specificationBuilder = specificationBuilder;
        _categoryCreateValidator = categoryCreateValidator;
        _categoryUpdateValidator = categoryUpdateValidator;
        _categoriesSearchValidator = categoriesSearchValidator;
        _logger = logger;
        _logService = logService;
        _categoryValidator = categoryValidator;
    }

    private async Task ClearCacheAsync(Guid id, CancellationToken token)
    {
        await _cache.RemoveAsync(CacheLabel, id, token);
        _logger.LogInformation("Кэш категории очищен.");
    }

    private async Task ValidateParentExistsAndThrowAsync(Guid parentId, CancellationToken token)
    {
        var exists = await _categoryValidator.ValidateExistsAsync(parentId, token);
        if (!exists)
        {
            throw new CategoryNotFoundException("Родительская категория с указанным идентификатором не была найдена.");
        }
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(CategoryCreate categoryCreate, CancellationToken token)
    {
        using var _ = _logService.PushProperty("CategoryCreate", categoryCreate, true);
        _logger.LogInformation("Запрос на создание категории.");
        
        _categoryCreateValidator.ValidateAndThrow(categoryCreate);
        if (categoryCreate.ParentId.HasValue)
        {
            await ValidateParentExistsAndThrowAsync(categoryCreate.ParentId.Value, token);
        }
        
        var id = await _repository.CreateAsync(categoryCreate, token);
        _logger.LogInformation("Категория успешно создана. Идентификатор созданной категории: {CategoryId}.", id);
        return id;
    }
    
    /// <inheritdoc />
    public async Task<CategoryInfo> GetInfoAsync(Guid id, CancellationToken token)
    {
        using var _ = _logService.PushProperty("CategoryId", id);
        _logger.LogInformation("Получение категории по идентификатору.");
        
        var info = await _cache.GetAsync<CategoryInfo>(CacheLabel, id, token);
        if (info != null)
        {
            _logger.LogInformation("Категория получена из кэша: {@CategoryInfo}.", info);
            return info;
        }
        info = await _repository.GetInfoAsync(id, token);
        _logger.LogInformation("Категория получена из базы данных: {@CategoryInfo}.", info);
        
        await _cache.SetAsync(CacheLabel, id, info, CacheExpirationTime, token);
        _logger.LogInformation("Информация о категории добавлена в кэш.");

        return info;
    }
    
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<CategoryInfo>> SearchAsync(CategoriesSearch search, CancellationToken token)
    {
        using var _ = _logService.PushProperty("CategoriesSearch", search, true);
        _logger.LogInformation("Поиск категорий по запросу.");
        
        _categoriesSearchValidator.ValidateAndThrow(search);
        if (search.FilterByParentId != null &&
            search.FilterByParentId.ParentId.HasValue)
        {
            await ValidateParentExistsAndThrowAsync(search.FilterByParentId.ParentId.Value, token);
        }
        
        var specification = _specificationBuilder.Build(search);
        _logger.LogInformation("Построена спецификация поиска категорий.");
        
        var categories = await _repository.GetBySpecificationWithPaginationAsync(
            specification: specification, 
            skip: search.Skip, 
            take: search.Take.GetValueOrDefault(0), 
            order: search.Order!, 
            token: token);
        _logger.LogInformation("Категории успешно получены.");
        
        return categories;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        using var _ = _logService.PushProperty("CategoryId", id);
        _logger.LogInformation("Запрос на удаление категории.");
        
        await ClearCacheAsync(id, token);
        
        await _repository.DeleteAsync(id, token);
        _logger.LogInformation("Категория успешно удалена из базы данных.");
    }
    
    /// <inheritdoc />
    public async Task<CategoryInfo> UpdateAsync(Guid id, CategoryUpdate categoryUpdate, CancellationToken token)
    {
        using (_logService.PushProperty("CategoryId", id))
        using (_logService.PushProperty("CategoryUpdate", categoryUpdate, true))
        {
            _logger.LogInformation("Запрос на обновление категории.");
        
            _categoryUpdateValidator.ValidateAndThrow(categoryUpdate);
        
            await ClearCacheAsync(id, token);
        
            var category = await _repository.UpdateAsync(id, categoryUpdate, token);
            _logger.LogInformation("Категория успешно обновлена.");
            
            return category; 
        }
    }
}