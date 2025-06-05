using CW_10_s30081.DTOs;
using CW_10_s30081.Exceptions;
using CW_10_s30081.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW_10_s30081.Controllers {
    
    [ApiController]
    [Route("[controller]")]
    public class TripsController(IDbService service) : Controller {
        [HttpGet]
        public async Task<IActionResult> GetAllTrips([FromQuery] int page = 1,[FromQuery] int pageSize = -1) {
            try {

                return Ok(await service.GetTripsWithClientsAndCountriesOrderedByDate(page,pageSize));
            } catch (NotFoundException e) {
                return NotFound(e.Message);
            }
        }
        [HttpPost("{idTrip}/clints")]
        public async Task<IActionResult> AssignClientToTrip([FromRoute] int idTrip, [FromBody]TripReservationPostDTO reservationPostDTO) {
            try {
                await service.AssignClientToTrip(idTrip, reservationPostDTO);
                return NoContent();
            } catch (NotFoundException e) {
                return NotFound(e.Message);
            }
        }
    }
}
