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
        string AssignSlot();

        /// <summary>
        /// Abstract Function For Unpark Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        ParkingDetails Unpark(string VehicalNumber);
    }
}
