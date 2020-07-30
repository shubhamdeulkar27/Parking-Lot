using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using CommonLayer.CustomExceptions;
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
        [Authorize(Roles = "Admin, Driver, Attendant ")]
        [HttpPost]
        [Route("Park")]
        public IActionResult Park([FromBody]ParkingDetails parkingDetails)
        {
            try
            {
                //Fetching Claim Form JWT.
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                parkingDetails.DriverName = identity.Name;

                //Throws Custom Exception If Fields are Null.
                if (parkingDetails.VehicalOwnerName == null || parkingDetails.VehicalNumber == null ||
                    parkingDetails.Brand == null || parkingDetails.Color == null || parkingDetails.DriverName == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception If Fields are Empty.
                if (parkingDetails.VehicalOwnerName == "" || parkingDetails.VehicalNumber == "" ||
                    parkingDetails.Brand == "" || parkingDetails.Color == "" || parkingDetails.DriverName == "")
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

                //Calling BL.
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
                //Throws Custom Exception If VehicalNumber Is Null;
                if (VehicalNumber == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception If VehicalNumber Is Not in Valid Format.
                if (!Regex.IsMatch(VehicalNumber, @"^[A-Z]{2}\s[0-9]{2}\s[A-Z]{1,2}\s[0-9]{4}$"))
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_VEHICAL_NUMBER_FORMAT.ToString() + " Please Enter Vehical In 'MH 01 AZ 2005' This Format.");
                }

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
        [Route("IsSlotAvialble")]
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
        [Route("Vehical/{VehicalNumber}")]
        public IActionResult GetVehicalByNumber([FromRoute] string VehicalNumber)
        {
            try
            {
                //Throws Custom Exception If VehicalNumber Is Null;
                if (VehicalNumber == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception If VehicalNumber Is Not in Valid Format.
                if (!Regex.IsMatch(VehicalNumber, @"^[A-Z]{2}\s[0-9]{2}\s[A-Z]{1,2}\s[0-9]{4}$"))
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_VEHICAL_NUMBER_FORMAT.ToString() + " Please Enter Vehical In 'MH 01 AZ 2005' This Format.");
                }

                //Calling BL.
                ParkingDetails details = this.parkingLotBL.GetVehicalByNumber(VehicalNumber);
                if(details!=null && details.Status == "Parked")
                {
                    return Ok(new { Success = true, Message = "Vehical Details Fetched Successful", Data = details });
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
        [Route("Vehicals/{Color}")]
        public IActionResult GetVehicalDetailsByColor([FromRoute] string Color)
        {
            try
            {
                //Throws Exception If Field is Null.
                if (Color == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Exception If Field is Empty.
                if (Color == "")
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

                //Calling BL.
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
        [Route("Vehicals/{Brand}")]
        public IActionResult GetVehicalDetailsByBrand([FromRoute] string Brand)
        {
            try
            {
                //Throw Exception When Field is Null.
                if (Brand == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Exception when field is empty.
                if (Brand == "")
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

                //Calling BL.
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
        [Route("Vehicals/{Brand}/{Color}")]
        public IActionResult GetVehicalDetailsByBrandAndColor([FromRoute] string Brand, [FromRoute] string Color)
        {
            try
            {
                //Throw Exception When Field is Null.
                if (Brand == null || Color == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Exception when field is empty.
                if (Brand == "" || Color == "")
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

                //Calling BL.
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

        /// <summary>
        /// Function To Find Vehicals Of Handicap Drivers.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Police, Security")]
        [HttpGet]
        [Route("Vehicals/{Slot}")]
        public IActionResult GetHandicapVehicalBySlot( [FromRoute]string Slot)
        {
            try
            {
                List<ParkingDetails> list;

                //Throw Exception When Field is Null.
                if (Slot == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throw Exception When Field is Empty.
                if (Slot == "")
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

                //If slot is invalid should throw exception.
                if (string.Equals(Slot, "A") || string.Equals(Slot, "B") || string.Equals(Slot, "C") || string.Equals(Slot, "D"))
                {
                    //Calling BL.
                    list = this.parkingLotBL.GetHandicapVehicalBySlot(Slot);
                }
                else
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_SLOT_EXCEPTION.ToString());
                }

                //Sending Responses.
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
        /// Function To Get All Vehicals.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Police, Security")]
        [HttpGet]
        [Route("Vehicals")]
        public IActionResult GetAllDetails()
        {
            try
            {
                List<ParkingDetails> list = this.parkingLotBL.GetAllDetails();
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
