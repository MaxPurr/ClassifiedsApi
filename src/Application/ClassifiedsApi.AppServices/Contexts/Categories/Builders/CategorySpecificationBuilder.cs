using ClassifiedsApi.AppServices.Contexts.Categories.Specifications;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Categories;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Builders;

///<inheritdoc />
public class CategorySpecificationBuilder : ICategorySpecificationBuilder
{
    ///<inheritdoc />
    public ISpecification<CategoryInfo> Build(CategoriesSearch search)
    {
        var specification = search.FilterByParentId == null 
            ? Specification<CategoryInfo>.True 
            : new ByParentIdSpecification(search.FilterByParentId.ParentId);
        if (search.NameFilter != null)
        {
            specification &= new NameFilterSpecification(search.NameFilter);
        }
        return specification;
    }
}