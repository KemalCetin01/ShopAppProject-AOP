using AutoMapper;
using ShopApp.Entities.Concrete;
using ShopApp.Entities.Dtos;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.AutoMappers
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();
            
            CreateMap<UserForLoginDto, UserForLoginModel>();
            CreateMap<UserForLoginModel, UserForLoginDto>();
   

        }
      
    }
}
