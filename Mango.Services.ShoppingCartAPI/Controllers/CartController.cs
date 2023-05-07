using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        protected ResponseDto _response;
        public CartController(ICartRepository cartRepository) {     
         
            _cartRepository= cartRepository;
            this._response = new ResponseDto();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<Object> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = await _cartRepository.GetCartByUserId(userId);
                _response.Result =cartDto;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() {  ex.ToString()};
            }
            return _response;
        }
        [HttpPost("AddCart")]
        public async Task<Object> AddCart(CartDto cartdto)
        {
            try
            {
                CartDto cartDto = await _cartRepository.CreateUpdateCart(cartdto);
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost("UpdateCart")]
        public async Task<Object> UpdateCart(CartDto cartdto)
        {
            try
            {
                CartDto cartDto = await _cartRepository.CreateUpdateCart(cartdto);
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost("RemoveCart")]
        public async Task<Object> RemoveCart([FromBody]int  cartId)
        {
            try
            {
                bool IsSuccess = await _cartRepository.RemovedFromCart(cartId);
                _response.Result = IsSuccess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
