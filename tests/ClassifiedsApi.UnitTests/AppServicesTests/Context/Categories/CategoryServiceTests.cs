using ClassifiedsApi.AppServices.Common.Services;
using ClassifiedsApi.AppServices.Contexts.Categories.Builders;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.AppServices.Contexts.Categories.Services;
using ClassifiedsApi.AppServices.Contexts.Categories.Validators;
using ClassifiedsApi.Contracts.Contexts.Categories;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using ClassifiedsApi.AppServices.Contexts.Categories.Specifications;
using ClassifiedsApi.AppServices.Exceptions.Categories;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Common;
using Shouldly;

namespace ClassifiedsApi.UnitTests.AppServicesTests.Context.Categories;

/// <summary>
/// Тесты сервиса категорий <see cref="CategoryService"/>.
/// </summary>
public class CategoryServiceTests
{
    private readonly CategoryService _service;
    
    private readonly Mock<ICategoryRepository> _repositoryMock;
    private readonly Mock<ISerializableCache> _cacheMock;
    private readonly Mock<ICategorySpecificationBuilder> _specificationBuilderMock;
    private readonly Mock<ICategoryValidator> _categoryValidatorMock;
    
    private readonly Mock<ILogger<CategoryService>> _loggerMock;
    private readonly Mock<IStructuralLoggingService> _logServiceMock;
    
    private readonly Mock<IValidator<CategoryCreate>> _categoryCreateValidatorMock;
    private readonly Mock<IValidator<CategoryUpdate>> _categoryUpdateValidatorMock;
    private readonly Mock<IValidator<CategoriesSearch>> _categoriesSearchValidatorMock;
    
    private readonly Fixture _fixture;
    private readonly CancellationToken _token;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryServiceTests"/>.
    /// </summary>
    public CategoryServiceTests()
    {
        _repositoryMock = new Mock<ICategoryRepository>();
        _cacheMock = new Mock<ISerializableCache>();
        _specificationBuilderMock = new Mock<ICategorySpecificationBuilder>();
        _categoryValidatorMock = new Mock<ICategoryValidator>();
        
        _loggerMock = new Mock<ILogger<CategoryService>>();
        _logServiceMock = new Mock<IStructuralLoggingService>();
        
        _categoryCreateValidatorMock = new Mock<IValidator<CategoryCreate>>();
        _categoryUpdateValidatorMock = new Mock<IValidator<CategoryUpdate>>();
        _categoriesSearchValidatorMock = new Mock<IValidator<CategoriesSearch>>();

        _service = new CategoryService(
            _repositoryMock.Object,
            _cacheMock.Object,
            _specificationBuilderMock.Object,
            _categoryValidatorMock.Object,
            _loggerMock.Object,
            _logServiceMock.Object,
            _categoryCreateValidatorMock.Object,
            _categoryUpdateValidatorMock.Object,
            _categoriesSearchValidatorMock.Object);
        
        _fixture = new Fixture();
        _token = new CancellationTokenSource().Token;
    }

    [Fact]
    public async Task CreateAsync_ParentCategoryNotExists_ShouldThrowCategoryNotFoundException()
    {
        // Arrange
        var parentId = _fixture.Create<Guid>();
        var categoryCreate = _fixture
            .Build<CategoryCreate>()
            .With(c => c.ParentId, parentId)
            .Create();
        _categoryValidatorMock
            .Setup(v => v.ValidateExistsAsync(parentId, _token))
            .ReturnsAsync(false);

        // Act
        var createCategoryAction = () => _service.CreateAsync(categoryCreate, _token);
        
        // Assert
        await createCategoryAction.ShouldThrowAsync<CategoryNotFoundException>();
        
        _categoryValidatorMock.Verify(v => v.ValidateExistsAsync(parentId, _token), Times.Once);
        _repositoryMock.Verify(r => r.CreateAsync(categoryCreate, _token), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ParentCategoryExists_ShouldReturnCreatedCategoryId()
    {
        // Arrange
        var parentId = _fixture.Create<Guid>();
        var categoryCreate = _fixture
            .Build<CategoryCreate>()
            .With(c => c.ParentId, parentId)
            .Create();
        _categoryValidatorMock
            .Setup(v => v.ValidateExistsAsync(parentId, _token))
            .ReturnsAsync(true);
        
        var expectedCategoryId = _fixture.Create<Guid>();
        _repositoryMock
            .Setup(r => r.CreateAsync(categoryCreate, _token))
            .ReturnsAsync(expectedCategoryId);
        
        // Act
        var actualCategoryId = await _service.CreateAsync(categoryCreate, _token);
        
        // Assert
        actualCategoryId.ShouldBe(expectedCategoryId);
        
        _categoryValidatorMock.Verify(v => v.ValidateExistsAsync(parentId, _token), Times.Once);
        _repositoryMock.Verify(r => r.CreateAsync(categoryCreate, _token), Times.Once);
    }

    [Fact]
    public async Task GetInfoAsync_CategoryExistsInCache_ShouldReturnFromCache()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        var expectedCategoryInfo = _fixture
            .Build<CategoryInfo>()
            .With(c => c.Id, categoryId)
            .Create();
        _cacheMock
            .Setup(c => c.GetAsync<CategoryInfo>(It.IsAny<string>(), categoryId, _token))
            .ReturnsAsync(expectedCategoryInfo);
        
        // Act
        var actualCategoryInfo = await _service.GetInfoAsync(categoryId, _token);
        
        // Assert
        actualCategoryInfo.ShouldBeEquivalentTo(expectedCategoryInfo);
        
        _cacheMock.Verify(c => c.GetAsync<CategoryInfo>(It.IsAny<string>(), categoryId, _token), Times.Once);
        _repositoryMock.Verify(r => r.GetInfoAsync(categoryId, _token), Times.Never);
        _cacheMock.Verify(c => c.SetAsync(It.IsAny<string>(), categoryId, expectedCategoryInfo, It.IsAny<TimeSpan>(), _token), Times.Never);
    }
    
    [Fact]
    public async Task GetInfoAsync_NotExists_ShouldThrowCategoryNotFoundException()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        _cacheMock
            .Setup(c => c.GetAsync<CategoryInfo>(It.IsAny<string>(), categoryId, _token))
            .ReturnsAsync(default(CategoryInfo));
        _repositoryMock
            .Setup(r => r.GetInfoAsync(categoryId, _token))
            .ThrowsAsync(new CategoryNotFoundException());
        
        // Act
        var getInfoAction = () => _service.GetInfoAsync(categoryId, _token);
        
        // Assert 
        await getInfoAction.ShouldThrowAsync<CategoryNotFoundException>();
        
        _cacheMock.Verify(c => c.GetAsync<CategoryInfo>(It.IsAny<string>(), categoryId, _token), Times.Once);
        _repositoryMock.Verify(r => r.GetInfoAsync(categoryId, _token), Times.Once);
        _cacheMock.Verify(c => c.SetAsync(It.IsAny<string>(), categoryId, It.IsAny<CategoryInfo>(), It.IsAny<TimeSpan>(), _token), Times.Never);
    }

    [Fact]
    public async Task GetInfoAsync_CategoryExistsInDB_ShouldReturnFromDB()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        var expectedCategoryInfo = _fixture
            .Build<CategoryInfo>()
            .With(c => c.Id, categoryId)
            .Create();
        _cacheMock
            .Setup(c => c.GetAsync<CategoryInfo>(It.IsAny<string>(), categoryId, _token))
            .ReturnsAsync(default(CategoryInfo));
        _repositoryMock
            .Setup(r => r.GetInfoAsync(categoryId, _token))
            .ReturnsAsync(expectedCategoryInfo);
        
        // Act
        var actualCategoryInfo = await _service.GetInfoAsync(categoryId, _token);
        
        // Assert
        actualCategoryInfo.ShouldBeEquivalentTo(expectedCategoryInfo);
        
        _cacheMock.Verify(c => c.GetAsync<CategoryInfo>(It.IsAny<string>(), categoryId, _token), Times.Once);
        _repositoryMock.Verify(r => r.GetInfoAsync(categoryId, _token), Times.Once);
        _cacheMock.Verify(c => c.SetAsync(It.IsAny<string>(), categoryId, expectedCategoryInfo, It.IsAny<TimeSpan>(), _token), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ParentCategoryNotExists_ShouldThrowCategoryNotFoundException()
    {
        // Arrange
        var parentId = _fixture.Create<Guid>();
        var filterByParentId = new FilterByParentId
        {
            ParentId = parentId
        };
        var skip = _fixture.Create<int?>();
        var take = _fixture.Create<int>();
        var order = _fixture.Create<CategoriesOrder>();
        var categoriesSearch = _fixture
            .Build<CategoriesSearch>()
            .With(s => s.Skip, skip)
            .With(s => s.Take, take)
            .With(s => s.FilterByParentId, filterByParentId)
            .With(s => s.Order, order)
            .Create();
        _categoryValidatorMock
            .Setup(v => v.ValidateExistsAsync(parentId, _token))
            .ReturnsAsync(false);
        
        // Act
        var searchAction = () => _service.SearchAsync(categoriesSearch, _token);

        // Assert
        await searchAction.ShouldThrowAsync<CategoryNotFoundException>();
        
        _categoryValidatorMock.Verify(v => v.ValidateExistsAsync(parentId, _token), Times.Once);
        _specificationBuilderMock.Verify(b => b.Build(categoriesSearch), Times.Never);
        _repositoryMock.Verify(r => r.GetBySpecificationWithPaginationAsync(
            It.IsAny<ISpecification<CategoryInfo>>(), skip, take, order, _token), Times.Never);
    }

    [Fact]
    public async Task SearchAsync_ParentCategoryExists_ShouldReturnCategories()
    {
        // Arrange
        var parentId = _fixture.Create<Guid>();
        var filterByParentId = new FilterByParentId
        {
            ParentId = parentId
        };
        var skip = _fixture.Create<int?>();
        var take = _fixture.Create<int>();
        var order = _fixture.Create<CategoriesOrder>();
        var categoriesSearch = _fixture
            .Build<CategoriesSearch>()
            .With(s => s.Skip, skip)
            .With(s => s.Take, take)
            .With(s => s.FilterByParentId, filterByParentId)
            .With(s => s.Order, order)
            .Create();
        _categoryValidatorMock
            .Setup(v => v.ValidateExistsAsync(parentId, _token))
            .ReturnsAsync(true);
        var specification = new ByParentIdSpecification(parentId);
        _specificationBuilderMock
            .Setup(b => b.Build(categoriesSearch))
            .Returns(specification);
        var expectedCategories = _fixture
            .CreateMany<CategoryInfo>(5)
            .ToList();
        _repositoryMock
            .Setup(r => r.GetBySpecificationWithPaginationAsync(specification, skip, take, order, _token))
            .ReturnsAsync(expectedCategories);
        
        // Act
        var actualCategories = await _service.SearchAsync(categoriesSearch, _token);
        
        // Assert
        actualCategories.ShouldBeEquivalentTo(expectedCategories);
        
        _categoryValidatorMock.Verify(v => v.ValidateExistsAsync(parentId, _token), Times.Once);
        _specificationBuilderMock.Verify(b => b.Build(categoriesSearch), Times.Once);
        _repositoryMock.Verify(r => r.GetBySpecificationWithPaginationAsync(specification, skip, take, order, _token), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_CategoryNotExists_ShouldThrowCategoryNotFoundException()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        _repositoryMock
            .Setup(r => r.DeleteAsync(categoryId, _token))
            .ThrowsAsync(new CategoryNotFoundException());
        
        // Act
        var deleteAction = () => _service.DeleteAsync(categoryId, _token); 
        
        // Assert
        await deleteAction.ShouldThrowAsync<CategoryNotFoundException>();
        
        _cacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>(), categoryId, _token), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(categoryId, _token), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_CategoryExists_ShouldSuccess()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        
        // Act
        await _service.DeleteAsync(categoryId, _token);
        
        // Assert
        _cacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>(), categoryId, _token), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(categoryId, _token), Times.Once);
    }
    
    [Fact]
    public async Task UpdateAsync_CategoryExists_ShouldReturnUpdatedCategory()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        var categoryUpdate = _fixture.Create<CategoryUpdate>();
        var expectedCategoryInfo = _fixture
            .Build<CategoryInfo>()
            .With(c => c.Id, categoryId)
            .With(c => c.Name, categoryUpdate.Name)
            .Create();
        _repositoryMock
            .Setup(r => r.UpdateAsync(categoryId, categoryUpdate, _token))
            .ReturnsAsync(expectedCategoryInfo);
        
        // Act
        var actualCategoryInfo = await _service.UpdateAsync(categoryId, categoryUpdate, _token);
        
        // Assert
        actualCategoryInfo.ShouldBeEquivalentTo(expectedCategoryInfo);
        
        _cacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>(), categoryId, _token), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(categoryId, categoryUpdate, _token), Times.Once);
    }
}