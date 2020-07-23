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
    }
}
