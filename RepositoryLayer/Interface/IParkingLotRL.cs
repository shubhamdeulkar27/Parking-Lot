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
        List<ParkingDetails> GetVehicalDetailsByColor(string vehicalColor);
    }
}
