using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ShopeeController : BaseApiController
    {
        public ShopeeController()
        {
            
        }

        [HttpPost("create-orders"), DisableRequestSizeLimit]
        public async Task<IActionResult> CreateOrders(IFormFile formFile)
        {
            try {
                var name = formFile.FileName;
                return Ok();
            } catch (Exception e) 
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }
    }
}