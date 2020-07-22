using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ParkingLot.Controllers
{
    /// <summary>
    /// Controller For Parking.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        /// <summary>
        /// IParkingLotBL Reference.
        /// </summary>
        private IParkingLotBL parkingLotBL;

        /// <summary>
        /// Constrcutor For Setting ParkingLotBL.
        /// </summary>
        /// <param name="parkingLotBL"></param>
        public ParkingController(IParkingLotBL parkingLotBL)
        {
            this.parkingLotBL = parkingLotBL;
        }

        /// <summary>
        /// Function For Handelling Park Request.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Park")]
        public IActionResult Park([FromBody]ParkingDetails parkingDetails)
        {
            try
            {
                var parkResponse = this.parkingLotBL.Park(parkingDetails);
                if(parkResponse != null)
                {
                    return Ok(new { Success = true, Message = "Vehical Parked", Data = parkResponse });
                }
                else if (parkResponse == null)
                {
                    return Conflict(new { Success = false, Message = "Vehical Is Already Parked" });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Lot Is Unavailable" });
                }
                
            }
            catch(Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }
    }
}
