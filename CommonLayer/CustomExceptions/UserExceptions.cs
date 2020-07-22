using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.CustomExceptions
{
    /// <summary>
    /// Class For User Model Exceptions.
    /// </summary>
    public class UserExceptions : Exception
    {
        /// <summary>
        /// Enum For Exception types.
        /// </summary>
        public enum ExceptionType
        {
            INVALID_USER_ROLE_EXCEPTION,
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
        public UserExceptions(UserExceptions.ExceptionType type, string message):base(message)
        {
            this.type = type;
        }
    }
}
