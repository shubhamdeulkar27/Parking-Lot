using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ParkingLot.Controllers
{
    /// <summary>
    /// Controller For User.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //UserBl Reference.
        private IUserBL userBL;

        /// <summary>
        /// Constructor for UserBL Reference.
        /// </summary>
        /// <param name="userBL"></param>
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        /// <summary>
        /// Function For Resgister User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Registration")]
        public IActionResult RegisterUser([FromBody] User user)
        {
            try
            {
                User responseUser = userBL.RegisterUser(user);
                if(responseUser!=null)
                {
                    return Ok(new { Success = true, Message = "Registration Successfull", Data = responseUser });
                }
                else
                {
                    return Conflict(new { Success = false, Message = "User Already Exists" });
                }
            }
            catch(Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        /// <summary>
        /// Function For User Login.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult LoginUser([FromBody] User user)
        {
            try
            {
                User responseUser = userBL.LoginUser(user);
                if (responseUser != null)
                {
                    return Ok(new { Success = true, Message = "Login Successfull", Data = responseUser });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Login Failed" });
                }
            }
            catch(Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }
    }
}
