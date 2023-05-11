using AutoMapper;
using Mango.Services.CouponAPI.Model;
using Mango.Services.CouponAPI.Model.Dto;

namespace Mango.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>().ReverseMap();
                //config.CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
                //config.CreateMap<CartDetailsDto, CartDetails>().ReverseMap();
                //config.CreateMap<CartDto, Cart>().ReverseMap();

            });

            return mappingConfig;
        }
    }
}
