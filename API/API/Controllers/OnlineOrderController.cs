using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OnlineOrderController : BaseApiController
    {
        public OnlineOrderController() 
        {}

        [HttpPost("create")]
        public async Task<ActionResult> CreateOrder()
        {
            
            return Ok();
        }
    }
}