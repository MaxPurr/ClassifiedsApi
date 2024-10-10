using System;
using ClassifiedsApi.Contracts.Contexts.Categories;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers.Base;

/// <summary>
/// Базовый контроллер администратора.
/// </summary>
public abstract class BaseAdminController : ControllerBase
{
    /// <summary>
    /// Метод для получения типизированной модели запроса категории.
    /// </summary>
    /// <param name="categoryId">Идентификатор категории.</param>
    /// <param name="model">Модель запроса.</param>
    /// <typeparam name="TModel">Тип модели запроса.</typeparam>
    /// <returns>Модель запроса категории.</returns>
    protected static CategoryRequest<TModel> GetCategoryRequest<TModel>(Guid categoryId, TModel model) where TModel : class
    {
        return new CategoryRequest<TModel>
        {
            CategoryId = categoryId,
            Model = model
        };
    }
}