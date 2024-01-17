using API.DTOs.Shopee;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ShopeeController : BaseApiController
    {
        public ShopeeController()
        {
            
        }

        [HttpPost("create-orders")]
        public async Task<IActionResult> CreateOrders(List<ShopeeOrderDTO> orders)
        {
            var order = orders;
            return Ok();
        }
    }
}