using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KisanGuru.Models.Entities
{
    [Table("tb_Users")]
    public class User
    {
        [Key]
        public string user_id { get; set; }

        [Required(ErrorMessage = "The user_name field is required.")]
        public string user_name { get; set; }

        [Required(ErrorMessage = "The aadhar_number field is required.")]
        public string aadhar_number { get; set; }

        [Required(ErrorMessage = "The email field is required.")]
        public string email { get; set; }

        [Required(ErrorMessage = "The phone_number field is required.")]
        public string phone_number { get; set; }

        [Required(ErrorMessage = "The address field is required.")]
        public string address { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        public string password { get; set; }

        [Required(ErrorMessage = "The role_id field is required.")]
        public string role_id { get; set; }

        [Required(ErrorMessage = "The inserted_by field is required.")]
        public string inserted_by { get; set; }

        [Required(ErrorMessage = "The updated_by field is required.")]
        public string updated_by { get; set; }

        // Navigation property
        [ForeignKey("role_id")]
        public virtual Role Role { get; set; }

        // Navigation Property
        public ICollection<Farmer> Farmers { get; set; }
    }
}
