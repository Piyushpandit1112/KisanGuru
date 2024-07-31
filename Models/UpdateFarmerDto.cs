namespace KisanGuru.Models
{
    public class UpdateFarmerDto
    {
        public required string farm_size { get; set; }
        public required string farm_location { get; set; }
        public required string pin_code { get; set; }
        public string? irrigation_method { get; set; }
        public string? soil_type { get; set; }
        public int? farming_experience { get; set; }
        public required string membership_status { get; set; }
        public string? language_preference { get; set; }
    }
}
