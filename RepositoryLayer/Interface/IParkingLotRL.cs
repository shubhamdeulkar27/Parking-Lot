using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IParkingLotRL
    {
        /// <summary>
        /// Abstract Function For Parking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        ParkingDetails Park(ParkingDetails parkingDetails);

        /// <summary>
        /// Abstract Function For Assigning Slot.
        /// </summary>
        /// <returns></returns>
        string AssignSlot(ParkingDetails parkingDetails);

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

        /// <summary>
        /// Abstract Function For Get Vehicals info By Brand and Color.
        /// </summary>
        /// <param name="brand"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        List<ParkingDetails> GetVehicalDetailsByBrandAndColor(string brand, string color);

        /// <summary>
        /// Abstract function for Get Handicap Vehical By Slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        List<ParkingDetails> GetHandicapVehicalBySlot(string slot);

        /// <summary>
        /// Abstract Funtion For Get All Vehicals Info.
        /// </summary>
        /// <returns></returns>
        List<ParkingDetails> GetAllDetails();

    }
}
