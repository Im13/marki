using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ShopeeController : BaseApiController
    {
        public ShopeeController()
        {
            
        }

        [HttpPost("create-orders")]
        public async Task<IActionResult> CreateOrders(IFormFile file)
        {
            try {
                var name = file.FileName;
                return Ok();
            } catch (Exception e) 
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }
    }
}