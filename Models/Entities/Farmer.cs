using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KisanGuru.Models.Entities

{
    [Table("tb_Farmers")]

    public class Farmer
    {
        [Key]
        public required string farmer_id { get; set; }


        public required string user_id { get; set; }
        // Navigation property
        [ForeignKey("user_id")]
        public virtual User RelatedUser { get; set; }
        public required string farm_size { get; set; }
        public required string farm_location {  get; set; }
        public required string pin_code { get; set; }
        public string? irrigation_method { get; set; }
        public string? soil_type { get; set; }
        public int? farming_experience { get; set; }
        public required string membership_status { get; set; }
        public DateTime? membership_expiry { get; set; }
        public string? language_preference { get; set; }
        public required string inserted_by { get; set; }
        public required string updated_by { get; set; }


    }
}
