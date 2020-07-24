using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IParkingLotBL
    {
        /// <summary>
        /// Abstract Function For Parking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        ParkingDetails Park(ParkingDetails parkingDetails);

        /// <summary>
        /// Abstract Function For Unpark Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        ParkingDetails Unpark(string VehicalNumber);

        /// <summary>
        /// Abstract Function For Check Parking Lot Status Function Implementation.
        /// </summary>
        /// <returns></returns>
        bool CheckLotStatus();

        /// <summary>
        /// Abstract Function For Find Vehical By Number Implementation.
        /// </summary>
        /// <param name="vehicalNumber"></param>
        /// <returns></returns>
        ParkingDetails GetVehicalByNumber(string vehicalNumber);
        
        /// <summary>
        /// Abstract Function For Get Vehicals info By Color Implementation.
        /// </summary>
        /// <param name="vehicalColor"></param>
        /// <returns></returns>
        List<ParkingDetails> GetVehicalDetailsByColor(string vehicalColor);

        /// <summary>
        /// Abstract Function For Get Vehicals info By Brand Implementation.
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        List<ParkingDetails> GetVehicalDetailsByBrand(string brand);
    }
}
