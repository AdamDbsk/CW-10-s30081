using CW_10_s30081.Exceptions;
using CW_10_s30081.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW_10_s30081.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ClientsController(IDbService service) : Controller {
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveClient([FromRoute] int id) {
            try {
                await service.RemoveClient(id);
                return NoContent();
            } catch (NotFoundException e) {
                return NotFound(e.Message);
            }
        }
        
    }


}
