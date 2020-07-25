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
                //Throws Custom Exception If Fields are Null.
                if(parkingDetails.VehicalOwnerName==null || parkingDetails.VehicalNumber==null ||
                    parkingDetails.Brand==null || parkingDetails.Color==null || parkingDetails.DriverName==null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception If Fields are Empty.
                if (parkingDetails.VehicalOwnerName == "" || parkingDetails.VehicalNumber == "" ||
                    parkingDetails.Brand == "" || parkingDetails.Color == "" || parkingDetails.DriverName == "")
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

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
                //Throws Custom Exception If VehicalNumber Is Null;
                if(VehicalNumber==null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception If VehicalNumber Is Not in Valid Format.
                if (!Regex.IsMatch(VehicalNumber, @"^[A-Z]{2}\s[0-9]{2}\s[A-Z]{1,2}\s[0-9]{4}$"))
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_VEHICAL_NUMBER_FORMAT.ToString() + "Please Enter Vehical In 'MH 01 AZ 2005' This Format.");
                }
 
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
                //Throws Custom Exception If VehicalNumber Is Null;
                if (VehicalNumber == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception If VehicalNumber Is Not in Valid Format.
                if (!Regex.IsMatch(VehicalNumber, @"^[A-Z]{2}\s[0-9]{2}\s[A-Z]{1,2}\s[0-9]{4}$"))
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_VEHICAL_NUMBER_FORMAT.ToString() + "Please Enter Vehical In 'MH 01 AZ 2005' This Format.");
                }

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
                //Throws Exception If Field is Null.
                if(vehicalColor == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }
                
                //Throws Exception If Field is Empty.
                if (vehicalColor == "")
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

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
                //Throw Exception When Field is Null.
                if(brand==null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Exception when field is empty.
                if (brand == "")
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

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
                //Throw Exception When Field is Null.
                if (brand == null || color == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Exception when field is empty.
                if (brand == "" || color== "")
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

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
                //Throw Exception When Field is Null.
                if (slot == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throw Exception When Field is Empty.
                if (slot == "")
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

                //If slot is invalid should throw exception.
                if(string.Equals(slot,"A") || string.Equals(slot, "B") || string.Equals(slot, "C") || string.Equals(slot, "D"))
                {
                    return this.parkingLotRL.GetHandicapVehicalBySlot(slot);
                }
                else
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_SLOT_EXCEPTION.ToString());
                }


            }
            catch(Exception exception)
            {
                throw exception;
            }
        }
    }
}
