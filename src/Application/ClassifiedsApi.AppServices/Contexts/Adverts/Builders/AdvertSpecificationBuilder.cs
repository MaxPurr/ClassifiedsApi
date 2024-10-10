using ClassifiedsApi.AppServices.Contexts.Adverts.Specifications;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Adverts;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Builders;

///<inheritdoc />
public class AdvertSpecificationBuilder : IAdvertSpecificationBuilder
{
    ///<inheritdoc />
    public ISpecification<ShortAdvertInfo> Build(AdvertsSearch search)
    {
        var specification = Specification<ShortAdvertInfo>.FromPredicate(advert =>
            search.IncludeDisabled.GetValueOrDefault(false) || !advert.Disabled);
        if (search.ExcludeWithoutImages.GetValueOrDefault(false))
        {
            specification &= new ExcludeWithoutImagesSpecification();
        }
        if (search.MinPrice.HasValue)
        {
            specification &= new MinPriceSpecification(search.MinPrice.Value);
        }
        if (search.MaxPrice.HasValue)
        {
            specification &= new MaxPriceSpecification(search.MaxPrice.Value);
        }
        if (search.TextFilter != null)
        {
            specification &= new TextFilterSpecification(search.TextFilter);
        }
        if (search.FilterByCategoryId != null)
        {
            specification &= new ByCategoryIdSpecification(search.FilterByCategoryId.GetValueOrDefault());
        }
        return specification;
    }
}