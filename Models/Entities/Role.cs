using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace KisanGuru.Models.Entities
{
    [Table("tb_Role")]
    public class Role
    {
        [Key]
        public required string role_id {  get; set; }
        public required string role_name { get; set; }
        public required string inserted_by { get; set; }
        public required string updated_by { get; set; }
        // Navigation property
        public ICollection<User> Users { get; set; }

    }
}
