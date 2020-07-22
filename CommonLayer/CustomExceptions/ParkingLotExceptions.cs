using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.CustomExceptions
{
    /// <summary>
    /// Class For Handelling Parking API's Exceptions.
    /// </summary>
    public class ParkingLotExceptions : Exception
    {
        public enum ExceptionType
        {
            NULL_FIELD_EXCEPTION,
            EMPTY_FIELD_EXCEPTION
        }

        /// <summary>
        /// Exception type Reference.
        /// </summary>
        ExceptionType type;

        /// <summary>
        /// Constrcutor For Setting Exception Type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public ParkingLotExceptions(ParkingLotExceptions.ExceptionType type, string message) : base(message)
        {
            this.type = type;
        }
    }
}
