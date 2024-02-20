using Nop.Plugin.Widgets.PharmaBase.Models;
using Nop.Plugin.Widgets.PharmaBase.Models.Attributes;
using Nop.Plugin.Widgets.PharmaBase.Models.MarketPlace;

namespace Nop.Plugin.Widgets.PharmaBase.Service
{
    public interface IMarketPlaceCategoryServiceInterface
    {
        Task<PagingResponseModel<List<MarketPlaceCategoriesResponse>>> GetCategoriesAsync();
        Task<PagingResponseModel<List<BrandResponse>>> GetBrandsAsync();
        Task<AttributesResponse> GetAttributesByCategoryId(int categoryId);
    }
}
