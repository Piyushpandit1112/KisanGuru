using System.ComponentModel.DataAnnotations;

namespace KisanGuru.Models
{
    public class AddConsultantDto
    {
        public required string user_id { get; set; }
        [Key]
        public required string consultant_id { get; set; }
        public string? expertise { get; set; }
        public int? experience { get; set; }
        public required string subscription_status { get; set; }
        public DateTime? subscription_expiry { get; set; }
        public required string inserted_by { get; set; }
        public required string updated_by { get; set; }
    }
}
