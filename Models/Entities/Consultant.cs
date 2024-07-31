using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KisanGuru.Models.Entities
{
    [Table("tb_Consultants")]
    public class Consultant
    {
        
        public required string user_id { get; set; }
        [Key]
        public required string consultant_id { get; set; }
        public string? expertise { get; set; }
        public int? experience { get; set; } = null;
        public required string subscription_status { get; set; }
        public DateTime? subscription_expiry { get; set; }
        public required string inserted_by { get; set; }
        public required string updated_by { get; set; }


    }
}
