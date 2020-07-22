using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.DBContext
{
    /// <summary>
    /// Class For DBContext.
    /// </summary>
    public class ParkingLotDBContext: DbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public ParkingLotDBContext(DbContextOptions<ParkingLotDBContext> options):base(options)
        {

        }

        /// <summary>
        /// DbSet Property For User Model. 
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// DbSet Property For Parking Details Model.
        /// </summary>
        public DbSet<ParkingDetails> ParkingDetails { get; set; }
    }
}
