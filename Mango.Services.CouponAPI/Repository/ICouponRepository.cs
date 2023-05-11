using Mango.Services.CouponAPI.Model.Dto;

namespace Mango.Services.CouponAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponytCode(string couponCode);
    }
}
