using BusinessLayer.Interface;
using CommonLayer.CustomExceptions;
using CommonLayer.Models;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer.Services
{
    /// <summary>
    /// Class For ParkingLotBL.
    /// </summary>
    public class ParkingLotBL : IParkingLotBL
    {
        /// <summary>
        /// ParkingLotRL Reference.
        /// </summary>
        private IParkingLotRL parkingLotRL;

        /// <summary>
        /// Constructor For Setting ParkingLotRL Instance.
        /// </summary>
        /// <param name="parkingLotRL"></param>
        public ParkingLotBL(IParkingLotRL parkingLotRL)
        {
            this.parkingLotRL = parkingLotRL;
        }

        /// <summary>
        /// Function For Parking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        public ParkingDetails Park(ParkingDetails parkingDetails)
        {
            try
            {
                return this.parkingLotRL.Park(parkingDetails);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Unpark Vehical.
        /// </summary>
        /// <param name="VehicalNumber"></param>
        /// <returns></returns>
        public ParkingDetails Unpark(string VehicalNumber)
        {
            try
            { 
                return this.parkingLotRL.Unpark(VehicalNumber);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Checking Parking Lot Status.
        /// </summary>
        /// <returns></returns>
        public bool CheckLotStatus()
        {
            try
            {
                return this.parkingLotRL.CheckLotStatus();
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function to Find The Car By Number.
        /// </summary>
        /// <param name="vehicalNumber"></param>
        /// <returns></returns>
        public ParkingDetails GetVehicalByNumber(string VehicalNumber)
        {
            try
            {
                return this.parkingLotRL.GetVehicalByNumber(VehicalNumber);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Get vehicals Details By Color.
        /// </summary>
        /// <param name="vehicalColor"></param>
        /// <returns></returns>
        public List<ParkingDetails> GetVehicalDetailsByColor(string vehicalColor)
        {
            try
            {
                return this.parkingLotRL.GetVehicalDetailsByColor(vehicalColor);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Getting Vehicals Of Specified Brand.
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        public List<ParkingDetails> GetVehicalDetailsByBrand(string brand)
        {
            try
            {
                return this.parkingLotRL.GetVehicalDetailsByBrand(brand);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function to get all vehical info of specified brand and color. 
        /// </summary>
        /// <param name="brand"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public List<ParkingDetails> GetVehicalDetailsByBrandAndColor(string brand, string color)
        {
            try
            {
                return this.parkingLotRL.GetVehicalDetailsByBrandAndColor(brand,color);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function for Get Handicap Vehical By Slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public List<ParkingDetails> GetHandicapVehicalBySlot(string slot)
        {
            try
            {
                return this.parkingLotRL.GetHandicapVehicalBySlot(slot);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function to Get All Vehical Details.
        /// </summary>
        /// <returns></returns>
        public List<ParkingDetails> GetAllDetails()
        {
            try
            {
                return this.parkingLotRL.GetAllDetails();
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }
    }
}
