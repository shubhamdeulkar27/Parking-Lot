using CommonLayer.Models;
using RepositoryLayer.DBContext;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    /// <summary>
    /// Class For ParkingLot RL. 
    /// </summary>
    public class ParkingLotRL : IParkingLotRL
    {
        //Constants.
        private const int TotalLotLimit = 100;
        private const int LotALimit = 25;
        private const int LotBLimit = 25;
        private const int LotCLimit = 25;
        private const int LotDLimit = 25;
        private const double RatePerHour = 10;

        /// <summary>
        /// DbContext Reference.
        /// </summary>
        private ParkingLotDBContext dBContext;

        /// <summary>
        /// Constructor For Setting DBContext Reference.
        /// </summary>
        /// <param name="dBContext"></param>
        public ParkingLotRL(ParkingLotDBContext dBContext)
        {
            this.dBContext= dBContext;
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
                //Checks If Vehical Is Already Parked.
                var existsCondition = dBContext.ParkingDetails.Where<ParkingDetails>
                    (p=> p.VehicalNumber.Equals(parkingDetails.VehicalNumber) && p.Status=="Parked").FirstOrDefault();

                if (existsCondition == null)
                {
                    //Assiging ParkingSlot 
                    parkingDetails.ParkingSlot = AssignSlot();
                    
                    //Checking Which Parking Slot is Assigned.
                    if(parkingDetails.ParkingSlot == "A" || parkingDetails.ParkingSlot == "B" ||
                        parkingDetails.ParkingSlot == "C" || parkingDetails.ParkingSlot == "D")
                    {
                        //Setting Status and DateTime
                        parkingDetails.Status = "Parked";
                        parkingDetails.ParkingDate = DateTime.Now;
                        
                        //Updating DataBase With The Data.
                        dBContext.ParkingDetails.Add(parkingDetails);
                        dBContext.SaveChanges();
                        
                    }
                    return parkingDetails;
                }
                else
                {
                    return existsCondition = null;
                }
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Assigning Slot.
        /// </summary>
        /// <returns></returns>
        public string AssignSlot()
        {
            try 
            {
                //Checking For Total Lot Vacancy.
                int limitCondition = dBContext.ParkingDetails.Where<ParkingDetails>(p => p.Status == "Parked").Count();
                if (limitCondition < TotalLotLimit)
                {
                    //Checking For Specific Lot Vacancy.
                    int lotAAvailable = dBContext.ParkingDetails.Where<ParkingDetails>(p => p.ParkingSlot == "A" && p.Status == "Parked").Count();
                    int lotBAvailable = dBContext.ParkingDetails.Where<ParkingDetails>(p => p.ParkingSlot == "B" && p.Status == "Parked").Count();
                    int lotCAvailable = dBContext.ParkingDetails.Where<ParkingDetails>(p => p.ParkingSlot == "C" && p.Status == "Parked").Count();
                    int lotDAvailable = dBContext.ParkingDetails.Where<ParkingDetails>(p => p.ParkingSlot == "D" && p.Status == "Parked").Count();

                    //Depending On Vaccancy, Slot will be Provided.
                    if (lotAAvailable < LotALimit && (lotAAvailable > lotBAvailable && lotBAvailable > lotCAvailable && lotCAvailable > lotDAvailable))
                    {
                        return "A";
                    }
                    else if (lotBAvailable < LotBLimit && (lotBAvailable > lotCAvailable && lotCAvailable > lotDAvailable && lotDAvailable > lotAAvailable))
                    {
                        return "B";
                    }
                    else if (lotCAvailable < LotCLimit && (lotCAvailable > lotDAvailable && lotDAvailable > lotAAvailable && lotAAvailable > lotBAvailable))
                    {
                        return "C";
                    }
                    else if (lotDAvailable < LotDLimit)
                    {
                        return "D";
                    }
                }
                return "Unavailable";
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Unpark Api.
        /// </summary>
        /// <param name="VehicalNumber"></param>
        /// <returns></returns>
        public ParkingDetails Unpark(string VehicalNumber)
        {
            try
            {
                ParkingDetails vehicalExists = dBContext.ParkingDetails.Where<ParkingDetails>
                (p => p.VehicalNumber.Equals(VehicalNumber)).FirstOrDefault();
                if (vehicalExists != null)
                {
                    if (vehicalExists.Status == "Parked")
                    {
                        vehicalExists.UnparkDate = DateTime.Now;
                        vehicalExists.TotalTime = vehicalExists.UnparkDate.Subtract(vehicalExists.ParkingDate).TotalHours;
                        vehicalExists.TotalAmount = vehicalExists.TotalTime * RatePerHour;
                        vehicalExists.Status = "Unparked";
                        vehicalExists.ParkingSlot = null;
                        dBContext.SaveChanges();
                        return vehicalExists;
                    }
                    else if (vehicalExists.Status == "Unparked")
                    {
                        vehicalExists.Status = "!Unparked";
                    }
                    return vehicalExists;
                }
                else
                {
                    return vehicalExists = null;
                }
            }
            catch(Exception exception)
            {
                throw exception;
            }
            
        }
    }
}
