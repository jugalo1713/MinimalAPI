using AutoMapper;
using MinimalAPI.Models;
using MinimalAPI.Models.DTO;

namespace MinimalAPI.Mappings
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Coupon, CouponDto>().ReverseMap();
            CreateMap<Coupon, CouponCreateDto>().ReverseMap();
            CreateMap<Coupon, CouponUpdateDto>().ReverseMap();
        }
    }
}
