using DataModel;
using Mapster;
using Model;

namespace Mapping
{
    public class ProductRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<Product, ProductDto>()
            //    .Map(dest => dest.Name, src => src.Name)
            //    .Map(dest => dest.Description, src => src.Description)
            //    .Map(dest => dest.Price, src => src.Price)
            //    .Map(dest => dest.CategoryId, src => src.CategoryId)
            //    .Map(dest => dest.Stock, src => src.Stock)
            //    .Map(dest => dest.ReleaseDate, src => src.ReleaseDate)
            //    .Map(dest => dest.ImageDto, src => src.Image != null ? Convert.ToBase64String(src.Image) : "");

            //config.NewConfig<ProductDto, Product>()
            //    .Ignore(dest => dest.Image);
        }
    }
}