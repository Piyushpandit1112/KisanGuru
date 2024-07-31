namespace KisanGuru.Models
{
    public class UpdateConsultantDto
    {
        public string? expertise { get; set; }
        public int? experience { get; set; }
        public required string subscription_status { get; set; }
        public DateTime? subscription_expiry { get; set; }
    }
}
