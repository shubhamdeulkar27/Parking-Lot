using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Class For User Model.
    /// </summary>
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required(ErrorMessage ="Username Required")]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression(@"^Admin|^Security$|^Police$|^Driver$", ErrorMessage ="Roles are Admin, Security, Police and Driver")]
        public string Role { get; set; }

        [Required(ErrorMessage ="Password is Required")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6,ErrorMessage ="Please Enter Minimum 6 Characters ")]
        public string Password { get; set; }

    }
}
