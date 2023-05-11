using Mango.Web.Models;
using System.Threading.Tasks;

namespace Mango.Web.Services.IServices
{
    public interface ICouponService
    {

        Task<T> GetCouponAsync<T>(string couponCode, string token = null);

    }
}
