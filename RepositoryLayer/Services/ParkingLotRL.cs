﻿using CommonLayer.Models;
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
        public int TotalLotLimit { get; set; } = 100;
        private const int LotALimit = 25;
        private const int LotBLimit = 25;
        private const int LotCLimit = 25;
        private const int LotDLimit = 25;
        private const double RatePerHour = 40;

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
                var parkingDetailsExists = dBContext.ParkingDetails.Where<ParkingDetails>
                    (p=> p.VehicalNumber.Equals(parkingDetails.VehicalNumber) && p.Brand.Equals(parkingDetails.Brand)).FirstOrDefault();

                if (parkingDetailsExists == null)
                {
                    //Assiging ParkingSlot 
                    parkingDetails.ParkingSlot = AssignSlot(parkingDetails);
                    
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
                else if(parkingDetailsExists.Status== "Unparked")
                {
                    //Assiging ParkingSlot 
                    parkingDetailsExists.ParkingSlot = AssignSlot(parkingDetails);

                    //Checking Which Parking Slot is Assigned.
                    if (parkingDetailsExists.ParkingSlot == "A" || parkingDetailsExists.ParkingSlot == "B" ||
                        parkingDetailsExists.ParkingSlot == "C" || parkingDetailsExists.ParkingSlot == "D")
                    {
                        //Setting Status and DateTime
                        parkingDetailsExists.Status = "Parked";
                        parkingDetailsExists.ParkingDate = DateTime.Now;
                        parkingDetailsExists.TotalTime = 0;
                        parkingDetailsExists.TotalAmount = 0;
                        parkingDetailsExists.DriverName = parkingDetails.DriverName;

                        //Updating DataBase With The Data.
                        //dBContext.ParkingDetails.Add(parkingDetails);
                        dBContext.SaveChanges();

                    }
                    return parkingDetailsExists;
                }
                else
                {
                    return parkingDetailsExists = null;
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
        public string AssignSlot(ParkingDetails parkingDetails)
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

                    if(parkingDetails.IsHandicap)
                    {
                        //Depending On Vaccancy, Slot will be Provided.
                        if (lotAAvailable < LotALimit )
                        {
                            return "A";
                        }
                        else if (lotBAvailable < LotBLimit )
                        {
                            return "B";
                        }
                        else if (lotCAvailable < LotCLimit )
                        {
                            return "C";
                        }
                        else if (lotDAvailable < LotDLimit)
                        {
                            return "D";
                        }
                    }
                    else
                    {
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
                        double Amount = vehicalExists.TotalTime * RatePerHour;
                        vehicalExists.TotalAmount = Amount > RatePerHour ? Amount : RatePerHour;
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

        /// <summary>
        /// Function For Checking Parking Lot Status.
        /// </summary>
        /// <returns></returns>
        public bool CheckLotStatus()
        {
            try
            {
                int parkedVehicalCount = dBContext.ParkingDetails.Where<ParkingDetails>(p => p.Status == "Parked").Count();
                bool status = parkedVehicalCount < TotalLotLimit ? true : false;
                return status;
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Find Vehical By Number.
        /// </summary>
        /// <param name="vehicalNumber"></param>
        /// <returns></returns>
        public ParkingDetails GetVehicalByNumber(string VehicalNumber)
        {
            try
            {
                ParkingDetails vehicalExists = dBContext.ParkingDetails.Where<ParkingDetails>
                    (p => p.VehicalNumber.Equals(VehicalNumber)).FirstOrDefault();
                return vehicalExists;
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Getting List Of Vehicals Of Specified Color.
        /// </summary>
        /// <param name="vehicalColor"></param>
        /// <returns></returns>
        public List<ParkingDetails> GetVehicalDetailsByColor(string vehicalColor)
        {
            try 
            {
                if(dBContext.ParkingDetails.Any(x => x.Color == vehicalColor))
                {
                    var data = (from ParkingDetails in dBContext.ParkingDetails
                                where ParkingDetails.Color == vehicalColor
                                && ParkingDetails.Status == "Parked"
                                select ParkingDetails).ToList();
                    return data;
                }
                else 
                {
                    return null;
                }
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Getting Vehicals By Brand Name.
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        public List<ParkingDetails> GetVehicalDetailsByBrand(string brand)
        {
            try
            {
                if (dBContext.ParkingDetails.Any(x => x.Brand == brand))
                {
                    var data = (from ParkingDetails in dBContext.ParkingDetails
                                where ParkingDetails.Brand == brand
                                && ParkingDetails.Status == "Parked"
                                select ParkingDetails).ToList();
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function for getting all vehical info of specified brand and color.
        /// </summary>
        /// <param name="brand"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public List<ParkingDetails> GetVehicalDetailsByBrandAndColor(string brand, string color)
        {
            try
            {
                if (dBContext.ParkingDetails.Any(p => p.Brand == brand && p.Color == color))
                {
                    var data = (from ParkingDetails in dBContext.ParkingDetails
                                where ParkingDetails.Brand == brand
                                && ParkingDetails.Color == color
                                && ParkingDetails.Status == "Parked"
                                select ParkingDetails).ToList();
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function to get handicap vehical details by slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public List<ParkingDetails> GetHandicapVehicalBySlot(string slot)
        {
            try
            {
                if(dBContext.ParkingDetails.Any<ParkingDetails>(p=> p.ParkingSlot == slot))
                {
                    var data = (from ParkingDetails in dBContext.ParkingDetails
                                where ParkingDetails.ParkingSlot == slot
                                && ParkingDetails.IsHandicap == true
                                && ParkingDetails.Status == "Parked"
                                select ParkingDetails).ToList();
                    return data;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function to get all vehicals details.
        /// </summary>
        /// <returns></returns>
        public List<ParkingDetails> GetAllDetails()
        {
            try
            {
                if (dBContext.ParkingDetails.Any<ParkingDetails>(p=> p.Status == "Parked"))
                {
                    var data = (from ParkingDetails in dBContext.ParkingDetails
                                where ParkingDetails.Status == "Parked"
                                select ParkingDetails).ToList();
                    return data;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
