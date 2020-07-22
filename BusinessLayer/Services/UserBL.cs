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
    /// Class For Business Layer For User.
    /// </summary>
    public class UserBL : IUserBL
    {
        /// <summary>
        /// RL Reference.
        /// </summary>
        private IUserRL userRL;

        /// <summary>
        /// Constructor For Setting UserRL Instance.
        /// </summary>
        /// <param name="userRL"></param>
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        /// <summary>
        /// Function For Password Encoding.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception exception)
            {
                throw new Exception("Error in base64Encode" + exception.Message);
            }
        }

        /// <summary>
        /// Function For Register User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User RegisterUser(User user)
        {
            try
            {
                //Throws Custom Exception When Fields are Null.
                if (user.UserName == null || user.Role == null || user.Password == null)
                {
                    throw new Exception(UserExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception When Fields are Empty Strings.
                if (user.UserName == ""  || user.Role == "" || user.Password == "" )
                {
                    throw new Exception(UserExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

                if(user.Email==null || user.Email=="")
                {
                    throw new Exception(UserExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception When Role is Invalid.
                if (user.Role.Equals(Roles.Admin) || user.Role.Equals(Roles.Driver) ||
                   user.Role.Equals(Roles.Police) || user.Role.Equals(Roles.Security))
                {
                    throw new Exception(UserExceptions.ExceptionType.INVALID_USER_ROLE_EXCEPTION.ToString());
                }

                user.Password = EncodePasswordToBase64(user.Password);
                return this.userRL.RegisterUser(user);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Login User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User LoginUser(User user)
        {
            try
            {
                //Throws Custom Exception When Fields are Null.
                if (user.Role == null || user.UserName == null || user.Password == null)
                {
                    throw new Exception(UserExceptions.ExceptionType.NULL_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception When Fields are Empty Strings.
                if (user.Role == "" || user.UserName == "" || user.Password == "")
                {
                    throw new Exception(UserExceptions.ExceptionType.EMPTY_FIELD_EXCEPTION.ToString());
                }

                //Throws Custom Exception When Role is Invalid.
                if (user.Role.Equals(Roles.Admin) || user.Role.Equals(Roles.Driver) ||
                   user.Role.Equals(Roles.Police) || user.Role.Equals(Roles.Security))
                {
                    throw new Exception(UserExceptions.ExceptionType.INVALID_USER_ROLE_EXCEPTION.ToString());
                }

                user.Password = EncodePasswordToBase64(user.Password);
                return this.userRL.LoginUser(user);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }


    }
}
