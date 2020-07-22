using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Interface for UserBL.
    /// </summary>
    public interface IUserBL
    {
        /// <summary>
        /// Abstact Function For Register User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User RegisterUser(User user);

        /// <summary>
        /// Abstract Function For Login user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User LoginUser(User user);
    }
}
