namespace KisanGuru.Models
{
    public class PatchConsultantDto
    {
        public string? expertise { get; set; }
        public int? experience { get; set; }
        public string? subscription_status { get; set; }
        public DateTime? subscription_expiry { get; set; }
        public string? updated_by { get; set; }
    }
}
