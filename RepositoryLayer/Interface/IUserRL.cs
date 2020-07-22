using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Interface for UserRL.
    /// </summary>
    public interface IUserRL
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
