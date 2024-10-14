using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Categories.Builders;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.AppServices.Extensions;
using ClassifiedsApi.Contracts.Contexts.Categories;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Services;

/// <inheritdoc />
public class CategoryService : ICategoryService
{
    private const string CacheLabel = "category";
    private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(5);
    
    private readonly ICategoryRepository _repository;
    private readonly IDistributedCache _cache;
    private readonly ICategorySpecificationBuilder _specificationBuilder;
    
    private readonly IValidator<CategoryCreate> _categoryCreateValidator;
    private readonly IValidator<CategoryUpdate> _categoryUpdateValidator;
    private readonly IValidator<CategoriesSearch> _categoriesSearchValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий файлов <see cref="ICategoryRepository"/>.</param>
    /// <param name="cache">Распределенный кэш <see cref="IDistributedCache"/>.</param>
    /// <param name="specificationBuilder">Строитель спецификаций для категорий <see cref="ICategorySpecificationBuilder"/>.</param>
    /// <param name="categoryCreateValidator">Валидатор модели создания категории.</param>
    /// <param name="categoryUpdateValidator">Валидатор модели обновления категории.</param>
    /// <param name="categoriesSearchValidator">Валидатор модели поиска категорий.</param>
    public CategoryService(
        ICategoryRepository repository, 
        IDistributedCache cache,
        ICategorySpecificationBuilder specificationBuilder,
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
    }

    private Task ClearCacheAsync(Guid id, CancellationToken token)
    {
        return _cache.RemoveAsync(CacheLabel, id, token);
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(CategoryCreate categoryCreate, CancellationToken token)
    {
        await _categoryCreateValidator.ValidateAndThrowAsync(categoryCreate, token);
        return await _repository.CreateAsync(categoryCreate, token);
    }
    
    /// <inheritdoc />
    public async Task<CategoryInfo> GetInfoAsync(Guid id, CancellationToken token)
    {
        var info = await _cache.GetAsync<CategoryInfo>(CacheLabel, id, token);
        if (info != null)
        {
            return info;
        }
        info = await _repository.GetInfoAsync(id, token);
        await _cache.SetAsync(CacheLabel, id, info, CacheExpirationTime, token);
        return info;
    }
    
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<CategoryInfo>> SearchAsync(CategoriesSearch search, CancellationToken token)
    {
        await _categoriesSearchValidator.ValidateAndThrowAsync(search, token);
        var specification = _specificationBuilder.Build(search);
        var categories = await _repository.GetBySpecificationWithPaginationAsync(
            specification: specification, 
            skip: search.Skip, 
            take: search.Take.GetValueOrDefault(0), 
            order: search.Order!, 
            token: token);
        return categories;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        await ClearCacheAsync(id, token);
        await _repository.DeleteAsync(id, token);
    }
    
    /// <inheritdoc />
    public async Task<CategoryInfo> UpdateAsync(Guid id, CategoryUpdate categoryUpdate, CancellationToken token)
    {
        _categoryUpdateValidator.ValidateAndThrow(categoryUpdate);
        
        await ClearCacheAsync(id, token);
        var category = await _repository.UpdateAsync(id, categoryUpdate, token);
        return category;
    }
    
    /// <inheritdoc />
    public async Task<bool> IsExistsAsync(Guid id, CancellationToken token)
    {
        var hasKey = await _cache.HasKeyAsync(CacheLabel, id, token);
        return hasKey || await _repository.IsExistsAsync(id, token);
    }
}