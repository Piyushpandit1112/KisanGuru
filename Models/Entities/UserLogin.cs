using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KisanGuru.Models.Entities
{
    // table name as per the database
    [Table("tb_UserLogin")]
    public class UserLogin
    {
        [Key]
        public required string user_name { get; set; }
        public required string password { get; set; }
    }
}
