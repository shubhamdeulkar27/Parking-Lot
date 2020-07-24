using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles= "Admin, Driver, Attendant ")]
        [HttpPost]
        [Route("Park")]
        public IActionResult Park([FromBody]ParkingDetails parkingDetails)
        {
            try
            {
                var parkResponse = this.parkingLotBL.Park(parkingDetails);
                if(parkResponse != null && parkResponse.ParkingSlot != "Unavailable")
                {
                    return Ok(new { Success = true, Message = "Vehical Parked", Data = parkResponse });
                }
                else if (parkResponse == null)
                {
                    return Conflict(new { Success = false, Message = "Vehical Is Already Parked" });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Lot Is Full" });
                }
                
            }
            catch(Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        /// <summary>
        /// Function For Vehical Unpark.
        /// </summary>
        /// <param name="VehicalNumber"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Driver")]
        [HttpPost]
        [Route("Unpark/{VehicalNumber}")]
        public IActionResult Unpark([FromRoute] string VehicalNumber)
        { 
            try
            {
                var unparkResponse = this.parkingLotBL.Unpark(VehicalNumber);
                if(unparkResponse != null && unparkResponse.Status=="Unparked")
                {
                    return Ok(new { Success = true, Message = "Vehical Unparked", Data = unparkResponse });
                }
                else if(unparkResponse != null && unparkResponse.Status == "!Unparked")
                {
                    return Conflict(new { Success = false, Message = "Vehical Already Unparked."});
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Vehical Not Found." });
                }
            }
            catch(Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        /// <summary>
        /// Function For Checking Parking Lot Status.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Admin, Owner, Security")]
        [HttpGet]
        [Route("LotStatus")]
        public IActionResult CheckLotStatus()
        { 
            try
            {
                bool status = this.parkingLotBL.CheckLotStatus();

                if(status)
                {
                    return Ok(new { Success = true, Message = "Lot Is Available" });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Lot Is Full" });
                }
            }
            catch(Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        /// <summary>
        /// Function To Find Vehical By Number.
        /// </summary>
        /// <param name="VehicalNumber"></param>
        /// <returns></returns>
        [Authorize(Roles ="Admin, Driver")]
        [HttpGet]
        [Route("FindByNumber/{VehicalNumber}")]
        public IActionResult GetVehicalByNumber([FromRoute] string VehicalNumber)
        {
            try
            {
                ParkingDetails details = this.parkingLotBL.GetVehicalByNumber(VehicalNumber);
                if(details!=null && details.Status == "Parked")
                {
                    return Ok(new { Success = true, Message = "Car Details Fetched Successful", Data = details });
                }
                else if (details !=null && details.Status == "Unparked")
                {
                    return NotFound(new { Success = false, Message = "Car Already Unparked" });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Car Not Found" });
                }
            }
            catch(Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        /// <summary>
        /// Function To Find Vehicals By Color.
        /// </summary>
        /// <param name="Color"></param>
        /// <returns></returns>
        [Authorize(Roles ="Admin, Police, Security, Owner")]
        [HttpGet]
        [Route("GetByColor/{Color}")]
        public IActionResult GetVehicalDetailsByColor([FromRoute] string Color)
        {
            try
            {
                var list = this.parkingLotBL.GetVehicalDetailsByColor(Color);
                if (list != null)
                {
                    return Ok(new { Success = true, Message = "Vehical Details Fetched Successful", Data = list });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "No Car Found" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        /// <summary>
        /// Function To Find Vehicals By Brand.
        /// </summary>
        /// <param name="Brand"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Police, Security, Owner")]
        [HttpGet]
        [Route("GetByBrand/{Brand}")]
        public IActionResult GetVehicalDetailsByBrand([FromRoute] string Brand)
        {
            try
            {
                var list = this.parkingLotBL.GetVehicalDetailsByBrand(Brand);
                if (list != null)
                {
                    return Ok(new { Success = true, Message = "Vehical Details Fetched Successful", Data = list });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "No Car Found" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

                /// <summary>
        /// Function To Find Vehicals By Color and Brand.
        /// </summary>
        /// <param name="Brand"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Police, Security")]
        [HttpGet]
        [Route("GetByBrandAndColor/{Brand}/{Color}")]
        public IActionResult GetVehicalDetailsByBrandAndColor([FromRoute] string Brand, [FromRoute] string Color)
        {
            try
            {
                var list = this.parkingLotBL.GetVehicalDetailsByBrandAndColor(Brand,Color);
                if (list != null)
                {
                    return Ok(new { Success = true, Message = "Vehical Details Fetched Successful", Data = list });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "No Car Found" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }
    }
}
