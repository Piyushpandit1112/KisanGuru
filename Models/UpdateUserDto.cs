namespace KisanGuru.Models
{
    public class UpdateUserDto
    {
        //If we want to make the some field as required and mandatory
        public required string user_name { get; set; }
        // Here string? means our phone value can be Null also
        public required string aadhar_number { get; set; }
        public required string email { get; set; }
        public required string phone_number { get; set; }
        public string? address { get; set; }
        public required string password { get; set; }
        public required string role_id { get; set; }
        public required string inserted_by { get; set; }
        public required string updated_by { get; set; }
    }
}
