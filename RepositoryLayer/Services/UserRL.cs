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
    /// Class For Repository Layer for User.
    /// </summary>
    public class UserRL : IUserRL
    {
        /// <summary>
        /// DBContext Reference.
        /// </summary>
        private ParkingLotDBContext dBContext;

        /// <summary>
        /// Constructor For Setting DBContext Instance.
        /// </summary>
        /// <param name="dBContext"></param>
        public UserRL(ParkingLotDBContext dBContext)
        {
            this.dBContext = dBContext;
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
                User userObject;
                if (user.Role.Equals("Driver"))
                {
                    userObject = dBContext.Users.Where<User>(u => u.UserName.Equals(user.UserName) || u.Email.Equals(user.Email)).FirstOrDefault();
                }
                else
                {
                    userObject = dBContext.Users.Where<User>(u => u.UserName.Equals(user.UserName) || u.Role.Equals(user.Role) || u.Email.Equals(user.Email)).FirstOrDefault();
                }

                //If User Not Found In Database then adds the user.
                if (userObject == null)
                {
                    dBContext.Users.Add(user);
                    dBContext.SaveChanges();
                    return user;
                }
                else 
                { 
                    return userObject=null; 
                }
            }
            catch(Exception exception)
            { 
                throw exception; 
            }
        }

        /// <summary>
        /// Function For LoginUser.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User LoginUser(User user)
        {
            try 
            {
                var userObject = dBContext.Users.Where<User>(u => u.UserName.Equals(user.UserName) && u.Password.Equals(user.Password)).FirstOrDefault();
                return userObject;
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

    }
}
