using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParkingLot.Controllers;
using RepositoryLayer.DBContext;
using RepositoryLayer.Interface;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace XunitTests
{
    /// <summary>
    /// Class For Unit Tests.
    /// </summary>
    public class Tests
    {
        //Controller Reference.
        UserController userController;

        //Reference BL and RL.
        private readonly IUserBL userBL;
        private readonly IUserRL userRL;

        //Reference Of DbContext and Configuration.
        public static DbContextOptions<ParkingLotDBContext> dbContextOptions { get; }
        private readonly IConfiguration configuration;

        //Connection String.
        public static string connectionString = "Server=localhost\\SQLEXPRESS;Database=ParkingLotDB;Trusted_Connection=True";

        //Model Instance.
        User user = new User();

        /// <summary>
        /// Static Constructor For Setting Static Field References.
        /// </summary>
        static Tests()
        {
            dbContextOptions = new DbContextOptionsBuilder<ParkingLotDBContext>()
                .UseSqlServer(connectionString).Options;
        }

        /// <summary>
        /// Constructor For Setting RL, BL, Controller Instances.
        /// </summary>
        public Tests()
        {
            var dbContext = new ParkingLotDBContext(dbContextOptions);

            userRL = new UserRL(dbContext);
            userBL = new UserBL(userRL);
            userController = new UserController(userBL);
        }

        /// <summary>
        /// Test Case For Register User Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void UserDetailsNullShouldReturnBadRequest()
        {
            //Setting Values.
            user.UserName = null;
            user.Email = null;
            user.Role = null;
            user.Password = null;
            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            bool Success = false;
            string Message = "NULL_FIELD_EXCEPTION";

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(Success, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }
    }
}
