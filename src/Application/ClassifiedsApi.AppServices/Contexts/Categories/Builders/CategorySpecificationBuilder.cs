using ClassifiedsApi.AppServices.Contexts.Categories.Specifications;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.AppServices.Specifications.Extensions;
using ClassifiedsApi.Contracts.Contexts.Categories;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Builders;

public class CategorySpecificationBuilder : ICategorySpecificationBuilder
{
    ///<inheritdoc />
    public ISpecification<CategoryInfo> Build(CategoriesSearch search)
    {
        ISpecification<CategoryInfo> specification;
        if (search.FilterByParentId == null)
        {
            specification = Specification<CategoryInfo>.True;
        }
        else
        {
            specification = new ByParentIdSpecification(search.FilterByParentId.ParentId);
        }
        if (search.NameFilter != null)
        {
            specification = specification.And(new NameFilterSpecification(search.NameFilter));
        }
        return specification;
    }
}