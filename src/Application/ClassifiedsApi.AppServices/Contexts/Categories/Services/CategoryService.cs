using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Categories.Builders;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.Contracts.Contexts.Categories;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Services;

/// <inheritdoc />
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly ICategorySpecificationBuilder _specificationBuilder;
    
    private readonly IValidator<CategoryCreate> _categoryCreateValidator;
    private readonly IValidator<CategoryRequest<CategoryUpdate>> _categoryUpdateRequestValidator;
    private readonly IValidator<CategoriesSearch> _categoriesSearchValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий файлов <see cref="ICategoryRepository"/>.</param>
    /// <param name="specificationBuilder">Строитель спецификаций для категорий <see cref="ICategorySpecificationBuilder"/>.</param>
    /// <param name="categoryCreateValidator">Валидатор модели создания категории.</param>
    /// <param name="categoryUpdateRequestValidator">Валидатор модели запроса на обновления категории.</param>
    /// <param name="categoriesSearchValidator">Валидатор модели поиска категорий.</param>
    public CategoryService(
        ICategoryRepository repository, 
        ICategorySpecificationBuilder specificationBuilder, 
        IValidator<CategoryCreate> categoryCreateValidator, 
        IValidator<CategoryRequest<CategoryUpdate>> categoryUpdateRequestValidator, 
        IValidator<CategoriesSearch> categoriesSearchValidator)
    {
        _repository = repository;
        _specificationBuilder = specificationBuilder;
        _categoryCreateValidator = categoryCreateValidator;
        _categoryUpdateRequestValidator = categoryUpdateRequestValidator;
        _categoriesSearchValidator = categoriesSearchValidator;
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(CategoryCreate categoryCreate, CancellationToken token)
    {
        await _categoryCreateValidator.ValidateAndThrowAsync(categoryCreate, token);
        return await _repository.CreateAsync(categoryCreate, token);
    }
    
    /// <inheritdoc />
    public Task<CategoryInfo> GetInfoAsync(Guid id, CancellationToken token)
    {
        return _repository.GetInfoAsync(id, token);
    }
    
    /// <inheritdoc />
    public Task<IReadOnlyCollection<CategoryInfo>> SearchAsync(CategoriesSearch search, CancellationToken token)
    {
        _categoriesSearchValidator.ValidateAndThrow(search);
        var specification = _specificationBuilder.Build(search);
        return _repository.GetBySpecificationWithPaginationAsync(specification, search.Skip, search.Take.GetValueOrDefault(0), search.Order!, token);
    }

    /// <inheritdoc />
    public Task DeleteAsync(Guid id, CancellationToken token)
    {
        return _repository.DeleteAsync(id, token);
    }
    
    /// <inheritdoc />
    public async Task<CategoryInfo> UpdateAsync(CategoryRequest<CategoryUpdate> updateRequest, CancellationToken token)
    {
        await _categoryUpdateRequestValidator.ValidateAndThrowAsync(updateRequest, token);
        var category = await _repository.UpdateAsync(updateRequest.CategoryId, updateRequest.Model, token);
        return category;
    }
    
    /// <inheritdoc />
    public Task<bool> IsExistsAsync(Guid id, CancellationToken token)
    {
        return _repository.IsExistsAsync(id, token);
    }
}