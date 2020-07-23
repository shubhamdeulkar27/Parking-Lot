﻿using BusinessLayer.Interface;
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

    }
}
