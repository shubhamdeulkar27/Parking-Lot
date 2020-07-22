using BusinessLayer.Interface;
using CommonLayer.CustomExceptions;
using CommonLayer.Models;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
