using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Model Class For Parking Details.
    /// </summary>
    public class ParkingDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReceiptNumber { get; set; }

        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage ="First Letter Must Be Capital")]
        [Required(ErrorMessage ="Please Enter Owner Name")]
        public string VehicalOwnerName { get; set; }

        [RegularExpression(@"^[A-Z]{2}\s[0-9]{2}\s[A-Z]{1,2}\s[0-9]{4}$",ErrorMessage ="Please Enter Vehical Number Like 'MH 01 AB 1111'")]
        [Required(ErrorMessage = "Please Enter The Vehical Number")]
        public string VehicalNumber { get; set; }

        [RegularExpression(@"^[A-Z][a-zA-Z]*$",ErrorMessage ="First Letter Must Be Capital")]
        [Required(ErrorMessage ="Please Enter Vehical Brand")]
        public string Brand  { get; set; }

        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "First Letter Must Be Capital")]
        [Required(ErrorMessage = "Please Enter Vehical Color")]
        public string Color { get; set; }

        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "First Letter Must Be Capital")]
        [Required(ErrorMessage = "Please Enter Driver Name")]
        public string DriverName { get; set; }

        public string ParkingSlot { get; set; }

        public System.DateTime ParkingDate { get; set; }
    }
}
