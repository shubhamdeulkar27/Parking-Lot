using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParkingLot.Controllers;
using RepositoryLayer.DBContext;
using RepositoryLayer.Interface;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
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
        ParkingController parkingController;

        //Reference BL and RL.
        private readonly IUserBL userBL;
        private readonly IUserRL userRL;
        private readonly IParkingLotBL parkingLotBL;
        private readonly IParkingLotRL parkingLotRL;

        //Reference Of DbContext and Configuration.
        public static DbContextOptions<ParkingLotDBContext> dbContextOptions { get; }
        private readonly IConfiguration configuration;
        private readonly ParkingLotDBContext dbContext;

        //Connection String.
        public static string connectionString = "Server=localhost\\SQLEXPRESS;Database=ParkingLotDB;Trusted_Connection=True";

        //Model Instance.
        User user = new User();
        ParkingDetails parkingDetails = new ParkingDetails();

        /// <summary>
        /// Static Constructor For Setting Static Field References.
        /// </summary>
        static Tests()
        {
            //Setting DBContext Options to use In Memory Database.
            dbContextOptions = new DbContextOptionsBuilder<ParkingLotDBContext>()
                .UseInMemoryDatabase(databaseName: "ParkingLotDB")
                .Options;
        }

        /// <summary>
        /// Constructor For Setting RL, BL, Controller Instances.
        /// </summary>
        public Tests()
        {
            this.dbContext = new ParkingLotDBContext(dbContextOptions);
            var myConfiguration = new Dictionary<string, string>
            {
                {"Jwt:Key", "ThisismySecretKey"},
                {"Jwt:Issuer", "ParkingLot.com"},
                {"Jwt:Audiance", "ParkingLot.com"},
                {"Logging:LogLevel:Default","Warning" },
                {"AllowedHosts","*" },
                {"ConnectionStrings:ParkingLotConnectionString","Server=localhost\\SQLEXPRESS;Database=ParkingLotDB;Trusted_Connection=True;" }
            };
            this.configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();

            userRL = new UserRL(dbContext);
            userBL = new UserBL(userRL);
            userController = new UserController(userBL,configuration);

            parkingLotRL = new ParkingLotRL(dbContext);
            parkingLotBL = new ParkingLotBL(parkingLotRL);
            parkingController = new ParkingController(parkingLotBL);
        }

        //Constants.
        private const bool  SuccessFalse = false;
        private const bool SuccessTrue = true;
        private const string Message_NullException = "NULL_FIELD_EXCEPTION";
        private const string Message_EmptyException = "EMPTY_FIELD_EXCEPTION";


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

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }

        /// <summary>
        /// Test Case For Register User Empty Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void UserDetailsEmptyShouldReturnBadRequest()
        {
            //Setting Values.
            user.UserName = "";
            user.Email = "";
            user.Role = "";
            user.Password = "";
            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);
        }

        /// <summary>
        /// Test Case For Register User Invalid Role Should Return BadRequest.
        /// </summary>
        [Fact]
        public void UserRoleInvalidShouldReturnBadRequest()
        {
            //Setting Values.
            user.UserName = "Shubham";
            user.Email = "shubhamdeulkar27@gmail.com";
            user.Role = "Tester";
            user.Password = "Sdeulkar2712";
            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "INVALID_USER_ROLE_EXCEPTION";

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Register User If User Exists Should Return Conflict.
        /// </summary>
        [Fact]
        public void UserExistsShouldReturnConflict()
        {
            //Setting Values.
            user.UserName = "admin@27";
            user.Name = "Admin";
            user.Email = "app.admin@gmail.com";
            user.Role = "Admin";
            user.Password = "Admin@27";
            var response = userController.RegisterUser(user) as ConflictObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "User Already Exists";

            //Asserting Values.
            Assert.IsType<ConflictObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Register User Valid Data Should Return Ok.
        /// </summary>
        [Fact]
        public void UserValidShouldReturnOk()
        {
            //Setting Values.
            user.UserName = "tester";
            user.Email = "app.tester@gmail.com";
            user.Role = "Driver";
            user.Password = "tester@27";
            user.Name = "Tester";
            var response = userController.RegisterUser(user) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();
            var responseData = dataResponse["Data"].ToString();

            //Expected Values.
            string Message = "Registration Successfull";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
            Assert.Equal(user.UserName, responseData);
        }

        /// <summary>
        /// Test Case For Login User Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void UserLoginNullFieldsShouldReturnBadRequest()
        {
            //Setting Values.
            user.UserName = null;
            user.Password = null;
            var response = userController.LoginUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }

        /// <summary>
        /// Test Case For Login User Empty Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void UserLoginEmptyFieldsShouldReturnBadRequest()
        {
            //Setting Values.
            user.UserName = "";
            user.Password = "";
            var response = userController.LoginUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);
        }

        /// <summary>
        /// Test Case For Login User Invalid Credentials Should Return NotFound.
        /// </summary>
        [Fact]
        public void UserLoginInvalidUserShouldReturnNotFound()
        {
            //Setting Values.
            user.UserName = "admin@55";
            user.Password = "Admin@55";
            var response = userController.LoginUser(user) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "User Not Found";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Login User Valid Credentials Should Return Ok.
        /// </summary>
        [Fact]
        public void UserLoginValidUserShouldReturnOk()
        {
            //Setting Values.
            dbContext.Users.Add(new User { Id = 4, UserName = "admin@27", Name = "Admin", Email = "app.admin@gmail.com", Role = "Admin", Password = "QWRtaW5AMjc=" });
            dbContext.SaveChanges();
            var userRL = new UserRL(dbContext);
            var userBL = new UserBL(userRL);
            var userController = new UserController(userBL, configuration);

            //Act.
            user.UserName = "admin@27";
            user.Password = "Admin@27";
            var response = userController.LoginUser(user) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();
            var responseData = dataResponse["Data"].ToString();

            //Expected Values.
            string Message = "Login Successfull";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
            Assert.Equal(user.UserName, responseData);
        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void Parking_NullFieldsShouldReturnBadRequest()
        {
            //Setting Values.
            parkingDetails.VehicalOwnerName = null;
            parkingDetails.VehicalNumber = null;
            parkingDetails.Brand = null;
            parkingDetails.Color = null;

            //Setting JWT Claim - Name
            Claim Name = new Claim(ClaimTypes.Name, "Ramesh");
            ClaimsIdentity identity = new ClaimsIdentity();
            identity.AddClaim(Name);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(identity);
            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(claimsPrincipal);
            parkingController.ControllerContext.HttpContext = contextMock.Object;

            //Act
            var response = parkingController.Park(parkingDetails) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }

        /// <summary>
        /// Test Case For Parking API Empty Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void Parking_EmptyFieldsShouldReturnBadRequest()
        {
            //Setting Values.
            parkingDetails.VehicalOwnerName = "";
            parkingDetails.VehicalNumber = "";
            parkingDetails.Brand = "";
            parkingDetails.Color = "";

            //Setting JWT Claim - Name
            Claim Name = new Claim(ClaimTypes.Name, "");
            ClaimsIdentity identity = new ClaimsIdentity();
            identity.AddClaim(Name);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(identity);
            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(claimsPrincipal);
            parkingController.ControllerContext.HttpContext = contextMock.Object;

            //Act
            var response = parkingController.Park(parkingDetails) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);
        }

        /// <summary>
        /// Test Case For Parking API Existing Vehical Should Return Conflict.
        /// </summary>
        [Fact]
        public void Parking_ExistingParkedVehicalShouldReturnConflict()
        {
            //Setting Values.
            parkingDetails.VehicalOwnerName = "Rahul";
            parkingDetails.VehicalNumber = "MH 01 AB 1234";
            parkingDetails.Brand = "Skoda";
            parkingDetails.Color = "Black";

            dbContext.ParkingDetails.Add(new ParkingDetails { ReceiptNumber = 2, VehicalOwnerName = "Rahul", VehicalNumber = "MH 01 AB 1234", Brand = "Skoda", Color = "Black", DriverName = "Ramesh", ParkingSlot = "D", Status = "Parked", TotalAmount = 0, TotalTime = 0, IsHandicap = false });
            dbContext.SaveChanges();
            var parkingRL = new ParkingLotRL(dbContext);
            var parkingBL = new ParkingLotBL(parkingRL);
            var parkingController = new ParkingController(parkingBL);

            //Setting JWT Claim - Name
            Claim Name = new Claim(ClaimTypes.Name, "Ramesh");
            ClaimsIdentity identity = new ClaimsIdentity();
            identity.AddClaim(Name);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(identity);
            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(claimsPrincipal);
            parkingController.ControllerContext.HttpContext = contextMock.Object;
            

            //Act
            var response = parkingController.Park(parkingDetails) as ConflictObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Vehical Is Already Parked";

            //Asserting Values.
            Assert.IsType<ConflictObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Parking API Valid Data Should Return Ok.
        /// </summary>
        [Fact]
        public void Parking_ValidDataShouldReturnOk()
        {
            //Setting Values.
            parkingDetails.VehicalOwnerName = "Mayur";
            parkingDetails.VehicalNumber = "MH 05 AB 1234";
            parkingDetails.Brand = "Tata";
            parkingDetails.Color = "White";

            //Setting JWT Claim - Name
            Claim Name = new Claim(ClaimTypes.Name, "Sanju");
            ClaimsIdentity identity = new ClaimsIdentity();
            identity.AddClaim(Name);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(identity);
            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(claimsPrincipal);
            parkingController.ControllerContext.HttpContext = contextMock.Object;

            //Act
            var response = parkingController.Park(parkingDetails) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();
            var responseData = dataResponse["Data"].ToObject<ParkingDetails>();

            //Expected Values.
            string Message = "Vehical Parked";
            string status = "Parked";
            
            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
            Assert.Equal(status, responseData.Status);
        }

        /// <summary>
        /// Test Case For Unpark API Null Fields Should Return Bad Request.
        /// </summary>
        [Fact]
        public void Parking_Unpark_NullFieldsShouldReturnBadRequest()
        {
            //Setting Values.
            string VehicalNumber = null;
            var response = parkingController.Unpark(VehicalNumber) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }

        /// <summary>
        /// Test Case For Unpark API Invalid Number Should Return Bad Request.
        /// </summary>
        [Fact]
        public void Parking_Unpark_InvalidNumberShouldReturnBadRequest()
        {
            //Setting Values.
            string VehicalNumber = "MH-01-AZ-2005";
            var response = parkingController.Unpark(VehicalNumber) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "INVALID_VEHICAL_NUMBER_FORMAT Please Enter Vehical In 'MH 01 AZ 2005' This Format.";

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Unpark API Valid Number Should Return Ok.
        /// </summary>
        [Fact]
        public void Parking_Unpark_ValidNumberShouldReturnOk()
        {
            
            //Setting Values.
            string VehicalNumber = "MH 01 AB 1234";
            Thread.Sleep(5000);
            var response = parkingController.Unpark(VehicalNumber) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();
            var responseData = dataResponse["Data"].ToObject<ParkingDetails>();

            //Expected Values.
            string Message = "Vehical Unparked";
            string status = "Unparked";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
            Assert.Equal(status, responseData.Status);
        }

        [Fact]
        public void Parking_StatusShouldReturnNotFound()
        {
            //Setting Data.
            var dbContext = new ParkingLotDBContext(dbContextOptions);
            IParkingLotRL parkingLotRL = new ParkingLotRL(dbContext) 
            {
                TotalLotLimit = 0
            };
            IParkingLotBL parkingLotBL = new ParkingLotBL(parkingLotRL);
            ParkingController parkingController = new ParkingController(parkingLotBL);
            
            //Act
            var response = parkingController.CheckLotStatus() as NotFoundObjectResult;
            var jsonResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = jsonResponse["Success"].ToObject<bool>();
            var responseMessage = jsonResponse["Message"].ToString();

            //Expected Values.
            string Message = "Lot Is Full";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Parking Slot Status API.
        /// </summary>
        [Fact]
        public void Parking_StatusShouldReturnOk()
        {
            //Act
            var response = parkingController.CheckLotStatus() as OkObjectResult;
            var jsonResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = jsonResponse["Success"].ToObject<bool>();
            var responseMessage = jsonResponse["Message"].ToString();

            //Expected Values.
            string Message = "Lot Is Available";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Number API-Null Fields Shour Return Bad Request. 
        /// </summary>
        [Fact]
        public void Parking_NullVehicalNumberShouldReturnBadRequest()
        {
            //Setting Values.
            string VehicalNumber = null;
            var response = parkingController.GetVehicalByNumber(VehicalNumber) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Number API-Invalid Number Format Should Return Bad Request. 
        /// </summary>
        [Fact]
        public void Parking_InvalidFormatedVehicalNumberShouldReturnBadRequest()
        {
            //Setting Values.
            string VehicalNumber = "MH-10-AB-12345";
            var response = parkingController.GetVehicalByNumber(VehicalNumber) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values
            string Message = "INVALID_VEHICAL_NUMBER_FORMAT Please Enter Vehical In 'MH 01 AZ 2005' This Format.";

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Number Unparked Vehical should Return Not Found. 
        /// </summary>
        [Fact]
        public void Parking_ValidVehicalNumberUnparkedShouldReturnNotFound()
        {
            //Setting Values.
            dbContext.ParkingDetails.Add(new ParkingDetails { ReceiptNumber = 9, VehicalOwnerName = "Pratik", VehicalNumber = "MH 07 PZ 1234",Brand = "Tata", Color = "White", DriverName = "Sanju", ParkingSlot = null, Status ="Unparked", TotalAmount = 40, TotalTime = 0.0015289095, IsHandicap = false });
            dbContext.SaveChanges();
            var parkingRL = new ParkingLotRL(dbContext);
            var parkingBL = new ParkingLotBL(parkingRL);
            var parkingController = new ParkingController(parkingBL);

            string VehicalNumber = "MH 07 PZ 1234";
            var response = parkingController.GetVehicalByNumber(VehicalNumber) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values
            string Message = "Car Already Unparked";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Number Invalid Vehical Number should Return Not Found. 
        /// </summary>
        [Fact]
        public void Parking_InvalidVehicalNumberShouldReturnNoFound()
        {
            //Setting Values.
            string VehicalNumber = "MH 01 AB 1555";
            var response = parkingController.GetVehicalByNumber(VehicalNumber) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values
            string Message = "Car Not Found";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Number Valid Vehical Number should Return Ok. 
        /// </summary>
        [Fact]
        public void Parking_ValidVehicalNumberShouldReturnOk()
        {
            //Setting Values.
            string VehicalNumber = "MH 01 AB 1234";
            var response = parkingController.GetVehicalByNumber(VehicalNumber) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();
            var responseData = dataResponse["Data"].ToObject<ParkingDetails>();

            //Expected Values
            string Message = "Vehical Details Fetched Successful";
            string Status = "Parked";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
            Assert.Equal(Status, responseData.Status);
        }

        /// <summary>
        /// Test Case For Get By Vehical Color - Null Fields should Return Bad Request. 
        /// </summary>
        [Fact]
        public void Parking_NullColorFieldShouldReturnBadRequest()
        {
            //Setting Values.
            string Color = null;
            var response = parkingController.GetVehicalDetailsByColor(Color) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();
            
            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Color - Empty Fields should Return Bad Request. 
        /// </summary>
        [Fact]
        public void Parking_EmptyColorFieldShouldReturnBadRequest()
        {
            //Setting Values.
            string Color = "";
            var response = parkingController.GetVehicalDetailsByColor(Color) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Color - Valid Vehical Of Color Not Exists should Return NotFound. 
        /// </summary>
        [Fact]
        public void Parking_NoVehicalOfColorShouldReturnNotFound()
        {
            //Setting Values.
            string Color = "Cyan";
            var response = parkingController.GetVehicalDetailsByColor(Color) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "No Car Found";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Color - Valid Vehical Color should Return Ok. 
        /// </summary>
        [Fact]
        public void Parking_VehicalOfColorExistsShouldReturnOk()
        {
            //Setting Values.
            string Color = "White";
            var response = parkingController.GetVehicalDetailsByColor(Color) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();
            var responseData = dataResponse["Data"].ToObject<List<ParkingDetails>>();

            //Expected Values.
            string Message = "Vehical Details Fetched Successful";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
            Assert.IsType<List<ParkingDetails>>(responseData);
        }

        /// <summary>
        /// Test Case For Get By Vehical Brand - Null Fields should Return Bad Request. 
        /// </summary>
        [Fact]
        public void Parking_NullBrandFieldShouldReturnBadRequest()
        {
            //Setting Values.
            string Brand = null;
            var response = parkingController.GetVehicalDetailsByBrand(Brand) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Brand - Empty Fields should Return Bad Request. 
        /// </summary>
        [Fact]
        public void Parking_EmptyBrandFieldShouldReturnBadRequest()
        {
            //Setting Values.
            string Brand = "";
            var response = parkingController.GetVehicalDetailsByBrand(Brand) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Brand - Valid Vehical Of Brand Not Exists should Return NotFound. 
        /// </summary>
        [Fact]
        public void Parking_NoVehicalOfBrandShouldReturnNotFound()
        {
            //Setting Values.
            string Brand = "Audi";
            var response = parkingController.GetVehicalDetailsByBrand(Brand) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "No Car Found";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Brand - Valid Vehical Brand should Return Ok. 
        /// </summary>
        [Fact]
        public void Parking_VehicalOfBrandExistsShouldReturnOk()
        {
            //Setting Values.
            string Brand = "Tata";
           
            //Act
            var response = parkingController.GetVehicalDetailsByBrand(Brand) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();
            var responseData = dataResponse["Data"].ToObject<List<ParkingDetails>>();

            //Expected Values.
            string Message = "Vehical Details Fetched Successful";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
            Assert.IsType<List<ParkingDetails>>(responseData);
        }

        
        /// <summary>
        /// Test Case For Get By Vehical Brand and Color - Null Fields should Return Bad Request. 
        /// </summary>
        [Fact]
        public void Parking_NullFieldShouldReturnBadRequest()
        {
            //Setting Values.
            string Brand = null;
            string Color = null;
            var response = parkingController.GetVehicalDetailsByBrandAndColor(Brand, Color) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Brand And Color - Empty Fields should Return Bad Request. 
        /// </summary>
        [Fact]
        public void Parking_EmptyFieldShouldReturnBadRequest()
        {
            //Setting Values.
            string Brand = "";
            string Color = "";
            var response = parkingController.GetVehicalDetailsByBrandAndColor(Brand, Color) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Brand and Color - Valid Vehical Of Brand and Color Not Exists should Return NotFound. 
        /// </summary>
        [Fact]
        public void Parking_NoVehicalOfBrandAndColorShouldReturnNotFound()
        {
            //Setting Values.
            string Brand = "Audi";
            string Color = "White";
            var response = parkingController.GetVehicalDetailsByBrandAndColor(Brand, Color) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "No Car Found";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Get By Vehical Brand - Valid Vehical Brand should Return Ok. 
        /// </summary>
        [Fact]
        public void Parking_VehicalOfBrandAndColorExistsShouldReturnOk()
        {
            //Setting Values.
            string Brand = "Tata";
            string Color = "White";

            //Act
            var response = parkingController.GetVehicalDetailsByBrandAndColor(Brand, Color) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();
            var responseData = dataResponse["Data"].ToObject<List<ParkingDetails>>();

            //Expected Values.
            string Message = "Vehical Details Fetched Successful";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
            Assert.IsType<List<ParkingDetails>>(responseData);
        }

        /// <summary>
        /// Test Case For Get Handicap Vehical By slot - Null Fields should Return Bad Request. 
        /// </summary>
        [Fact]
        public void Parking_NullSlotShouldReturnBadRequest()
        {
            //Setting Values.
            string Slot = null;
            var response = parkingController.GetHandicapVehicalBySlot(Slot) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }

        /// <summary>
        /// Test Case For Get Handicap Vehical By slot - Empty Fields should Return Bad Request. 
        /// </summary>
        [Fact]
        public void Parking_EmptySlotShouldReturnBadRequest()
        {
            //Setting Values.
            string Slot = "";
            var response = parkingController.GetHandicapVehicalBySlot(Slot) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);
        }

        /// <summary>
        /// Test Case For Get Handicap Vehical By slot - Invalid Slot should Return Bad Request. 
        /// </summary>
        [Fact]
        public void Parking_InvalidSlotShouldReturnBadRequest()
        {
            //Setting Values.
            string Slot = "Z";
            var response = parkingController.GetHandicapVehicalBySlot(Slot) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "INVALID_SLOT_EXCEPTION";

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Get Handicap Vehical By slot -  No Vehical Parked should Return NotFound. 
        /// </summary>
        [Fact]
        public void Parking_NoVehicalParkedShouldReturnNotFound()
        {
            //Setting Values.
            string Slot = "B";
            var response = parkingController.GetHandicapVehicalBySlot(Slot) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "No Car Found";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Get Handicap Vehical By Slot - Valid Slot should Return Ok. 
        /// </summary>
        [Fact]
        public void Parking_ValidSlotShouldReturnOk()
        {
            //Setting Values.
            string Slot = "D";

            //Act
            var response = parkingController.GetHandicapVehicalBySlot(Slot) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();
            var responseData = dataResponse["Data"].ToObject<List<ParkingDetails>>();

            //Expected Values.
            string Message = "Vehical Details Fetched Successful";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
            Assert.IsType<List<ParkingDetails>>(responseData);
        
        }

        /// <summary>
        /// Test Case For Get All Vehical API - should Return Ok. 
        /// </summary>
        [Fact]
        public void Parking_GetAllShouldReturnOk()
        {
            //Act
            var response = parkingController.GetAllDetails() as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();
            var responseData = dataResponse["Data"].ToObject<List<ParkingDetails>>();

            //Expected Values.
            string Message = "Vehical Details Fetched Successful";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
            Assert.IsType<List<ParkingDetails>>(responseData);
        }
    }
}
