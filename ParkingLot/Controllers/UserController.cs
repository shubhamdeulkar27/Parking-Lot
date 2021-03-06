﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using CommonLayer.CustomExceptions;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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

        //IConfiguration Reference for JWT.
        private IConfiguration configuration;


        /// <summary>
        /// Constructor for UserBL Reference.
        /// </summary>
        /// <param name="userBL"></param>
        public UserController(IUserBL userBL, IConfiguration configuration)
        {
            this.userBL = userBL;
            this.configuration = configuration;
        }

        /// <summary>
        /// Function For Resgister User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Registration")]
        public IActionResult RegisterUser([FromBody] User user)
        {
            try
            {
                //Null Instace For UserResponse.
                User responseUser = null;

                //Throws Custom Exception When Fields are Null.
                if (user.UserName == null || user.Role == null || user.Password == null || user.Email == null)
                {
                    throw new Exception(UserExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception When Fields are Empty Strings.
                if (user.UserName == "" || user.Role == "" || user.Password == "" || user.Email == "")
                {
                    throw new Exception(UserExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception When Role is Invalid.
                if (user.Role.Equals(Roles.Admin.ToString()) || user.Role.Equals(Roles.Driver.ToString()) ||
                   user.Role.Equals(Roles.Police.ToString()) || user.Role.Equals(Roles.Security.ToString()) ||
                   user.Role.Equals(Roles.Owner.ToString()) || user.Role.Equals(Roles.Attendant.ToString()))
                {
                    responseUser = userBL.RegisterUser(user);
                }
                else
                {
                    throw new Exception(UserExceptions.ExceptionType.INVALID_USER_ROLE_EXCEPTION.ToString());
                }

                if(responseUser!=null)
                {
                    return Ok(new { Success = true, Message = "Registration Successfull", Data = responseUser.UserName });
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
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult LoginUser([FromBody] User user)
        {
            try
            {
                //Throws Custom Exception When Fields are Null.
                if (user.UserName == null || user.Password == null)
                {
                    throw new Exception(UserExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception When Fields are Empty Strings.
                if (user.UserName == "" || user.Password == "")
                {
                    throw new Exception(UserExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

                User responseUser = userBL.LoginUser(user);
                if (responseUser != null)
                {
                    var tokenString = GenerateJsonWebToken(responseUser);
                    return Ok(new { Success = true, Message = "Login Successfull", Data = responseUser.UserName , Token = tokenString });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "User Not Found" });
                }
            }
            catch(Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        /// <summary>
        /// Function For Generating Jwt Token.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GenerateJsonWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audiance"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
