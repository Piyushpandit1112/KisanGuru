using Microsoft.EntityFrameworkCore;
using KisanGuru.Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Internal;
using KisanGuru.Models;
namespace KisanGuru.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserLogin> UserLogins {get; set;}
        // Method to map the stored procedure
        public IList<User> GetUserByRole(string role_id)
        {
            var role_id_param = new SqlParameter("@RoleID", role_id);

            return Users.FromSqlRaw("EXEC sp_GetUserRole @RoleID", role_id_param).ToList();
        }

        //For inserting in User

        public void InsertUser(
           string user_id,
           string user_name,
           string aadhar_number,
           string email,
           string phone_number,
           string address,
           string password,
           string role_id,
           string inserted_by,
           string updated_by)
        {
            var parameters = new[]
            {
                new SqlParameter("@user_id", user_id),
                new SqlParameter("@user_name", user_name),
                new SqlParameter("@aadhar_number", aadhar_number),
                new SqlParameter("@email", email),
                new SqlParameter("@phone_number", phone_number),
                new SqlParameter("@address", address ?? (object)DBNull.Value), // handle null address
                new SqlParameter("@password", password),
                new SqlParameter("@role_id", role_id),
                new SqlParameter("@inserted_by", inserted_by),
                new SqlParameter("@updated_by", updated_by)
            };

            Database.ExecuteSqlRaw("EXEC sp_SingleInsertUser @user_id, @user_name, @aadhar_number, @email, @phone_number, @address, @password, @role_id, @inserted_by, @updated_by", parameters);
        }

        // For updating in user

        public void UpdateUser(
            string user_id,
            string user_name,
            string aadhar_number,
            string email,
            string phone_number,
            string address,
            string password,
            string role_id,
            string updated_by)
        {
            var parameters = new[]
            {
                new SqlParameter("@user_id", user_id),
                new SqlParameter("@user_name", user_name),
                new SqlParameter("@aadhar_number", aadhar_number),
                new SqlParameter("@email", email),
                new SqlParameter("@phone_number", phone_number),
                new SqlParameter("@address", address ?? (object)DBNull.Value), // handle null address
                new SqlParameter("@password", password),
                new SqlParameter("@role_id", role_id),
                new SqlParameter("@updated_by", updated_by)
            };

            Database.ExecuteSqlRaw("EXEC sp_SingleUpdateUser @user_id, @user_name, @aadhar_number, @email, @phone_number, @address, @password, @role_id, @updated_by", parameters);
        }

        //when we want to patch / or update our user record
        public void PatchUser(string user_id, PatchUserDto patchUserDto)
        {
            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@user_id", user_id),
            new SqlParameter("@user_name", patchUserDto.user_name ?? (object)DBNull.Value),
            new SqlParameter("@aadhar_number", patchUserDto.aadhar_number ?? (object)DBNull.Value),
            new SqlParameter("@email", patchUserDto.email ?? (object)DBNull.Value),
            new SqlParameter("@phone_number", patchUserDto.phone_number ?? (object)DBNull.Value),
            new SqlParameter("@address", patchUserDto.address ?? (object)DBNull.Value),
            new SqlParameter("@password", patchUserDto.password ?? (object)DBNull.Value),
            new SqlParameter("@role_id", patchUserDto.role_id ?? (object)DBNull.Value),
            new SqlParameter("@updated_by", patchUserDto.updated_by ?? (object)DBNull.Value)
        };

            Database.ExecuteSqlRaw("EXEC sp_PatchUser @user_id, @user_name, @aadhar_number, @email, @phone_number, @address, @password, @role_id, @updated_by", parameters.ToArray());
        }


        //When we want to delete a useres
        public void DeleteUser(string user_id)
        {
            var parameter = new SqlParameter("@user_id", user_id);

            Database.ExecuteSqlRaw("EXEC sp_SingleDeleteUser @user_id", parameter);
        }
        // When we want to read all the users
        public IList<User> ReadAllUsers()
        {
            return Users.FromSqlRaw("EXEC sp_ReadAllUsers").ToList();
        }


        //When we want to insert data in consultant table
        public void InsertConsultant(
            string user_id,
            string consultant_id,
            string expertise,
            int? experience,
            string subscription_status,
            DateTime? subscription_expiry,  // Nullable DateTime
            string inserted_by,
            string updated_by)
        {
            var parameters = new[]
            {
                new SqlParameter("@user_id", user_id),
                new SqlParameter("@consultant_id", consultant_id),
                new SqlParameter("@expertise", expertise),
                new SqlParameter("@experience", experience),
                new SqlParameter("@subscription_status", subscription_status),
                new SqlParameter("@subscription_expiry", subscription_expiry.HasValue ? (object)subscription_expiry.Value : DBNull.Value),  // Handle null DateTime
                new SqlParameter("@inserted_by", inserted_by),
                new SqlParameter("@updated_by", updated_by)
            };

            Database.ExecuteSqlRaw("EXEC sp_SingleInsertConsultant @user_id, @consultant_id, @expertise, @experience, @subscription_status, @subscription_expiry, @inserted_by, @updated_by", parameters);
        }

        // Inside your ApplicationDbContext class

        // Method to patch (partially update) a consultant record
        public void PatchConsultant(string consultant_id, PatchConsultantDto patchConsultantDto)
        {
            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@consultant_id", consultant_id),
            new SqlParameter("@expertise", patchConsultantDto.expertise ?? (object)DBNull.Value),
            new SqlParameter("@experience", patchConsultantDto.experience.HasValue ? patchConsultantDto.experience.Value : (object)DBNull.Value),
            new SqlParameter("@subscription_status", patchConsultantDto.subscription_status ?? (object)DBNull.Value),
            new SqlParameter("@subscription_expiry", patchConsultantDto.subscription_expiry.HasValue ? (object)patchConsultantDto.subscription_expiry.Value : DBNull.Value),
            new SqlParameter("@updated_by", patchConsultantDto.updated_by)
        };

            Database.ExecuteSqlRaw("EXEC sp_PatchConsultant @consultant_id, @expertise, @experience, @subscription_status, @subscription_expiry, @updated_by", parameters.ToArray());
        }








        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().ToTable("tb_Users");
        //  //  modelBuilder.Entity<Farmer>().ToTable("tb_Farmers");
        //    base.OnModelCreating(modelBuilder);
        //}




    }
}
