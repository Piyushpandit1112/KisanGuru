using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KisanGuru.Data;
using KisanGuru.Models.Entities;
using KisanGuru.Models;
using CsvHelper;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace KisanGuru.Controllers
{
    [Route("v1/api/kisan_mitra/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ApplicationDbContext dbContext, ILogger<UsersController> logger)
        {
            this.dbContext = dbContext;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("get_all_users")]
        public IActionResult GetAllUsers(int page = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Getting all users, Page: {Page}, PageSize: {PageSize}", page, pageSize);

                int skip = (page - 1) * pageSize;
                var users = dbContext.Users
                                     .Skip(skip)
                                     .Take(pageSize)
                                     .ToList();

                return Ok(new
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = dbContext.Users.Count(),
                    Users = users,
                    Message = "Authenticated with JWT."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving users: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{user_id}")]
        public IActionResult GetUserById(string user_id)
        {
            try
            {
                _logger.LogInformation("Getting user by ID: {UserId}", user_id);

                var user = dbContext.Users.Find(user_id);
                if (user is null)
                {
                    _logger.LogWarning("User not found, ID: {UserId}", user_id);
                    return NotFound("You have entered wrong id");
                }
                return Ok(user  + "Authenticated with JWT");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by ID: {UserId}", user_id);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving user: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("insert_users")]

        public IActionResult AddUser(AddUserDto addUserDto)
        {
            try
            {
                _logger.LogInformation("Adding new user with ID: {UserId}", addUserDto.user_id);

                dbContext.InsertUser(
                    addUserDto.user_id,
                    addUserDto.user_name,
                    addUserDto.aadhar_number,
                    addUserDto.email,
                    addUserDto.phone_number,
                    addUserDto.address,
                    addUserDto.password,
                    addUserDto.role_id,
                    addUserDto.inserted_by,
                    addUserDto.updated_by
                );

                var user = dbContext.Users.Find(addUserDto.user_id);
                return Ok(user + "Authenticated with JWT");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user with ID: {UserId}", addUserDto.user_id);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding user: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("BulkInsertFromCsv")]
        public IActionResult BulkInsertFromCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded for bulk insert");
                return BadRequest("No file uploaded");
            }

            try
            {
                var errorMessages = new List<string>();

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    using (var reader = new StreamReader(stream))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = csv.GetRecords<AddUserDto>().ToList();

                        foreach (var addUserDto in records)
                        {
                            try
                            {
                                dbContext.InsertUser(
                                    addUserDto.user_id,
                                    addUserDto.user_name,
                                    addUserDto.aadhar_number,
                                    addUserDto.email,
                                    addUserDto.phone_number,
                                    addUserDto.address,
                                    addUserDto.password,
                                    addUserDto.role_id,
                                    addUserDto.inserted_by,
                                    addUserDto.updated_by
                                );
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error inserting user with ID: {UserId}", addUserDto.user_id);
                                errorMessages.Add($"Error inserting user with ID {addUserDto.user_id}: {ex.Message}");
                            }
                        }
                    }
                }

                if (errorMessages.Any())
                {
                    _logger.LogWarning("Errors occurred during bulk insert: {ErrorMessages}", errorMessages);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Errors = errorMessages });
                }

                _logger.LogInformation("Users have been successfully inserted");
                return Ok("Users have been successfully inserted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing bulk insert file");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error processing file: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("{user_id}")]
        public IActionResult UpdateUser(string user_id, UpdateUserDto updateUserDto)
        {
            try
            {
                _logger.LogInformation("Updating user with ID: {UserId}", user_id);

                var user = dbContext.Users.Find(user_id);
                if (user == null)
                {
                    _logger.LogWarning("No record found with ID: {UserId}", user_id);
                    return NotFound("There is no record with this ID");
                }

                user.user_name = updateUserDto.user_name;
                user.aadhar_number = updateUserDto.aadhar_number;
                user.email = updateUserDto.email;
                user.phone_number = updateUserDto.phone_number;
                user.address = updateUserDto.address;
                user.password = updateUserDto.password;
                user.role_id = updateUserDto.role_id;
                user.updated_by = updateUserDto.updated_by;

                dbContext.UpdateUser(
                    user_id,
                    updateUserDto.user_name,
                    updateUserDto.aadhar_number,
                    updateUserDto.email,
                    updateUserDto.phone_number,
                    updateUserDto.address,
                    updateUserDto.password,
                    updateUserDto.role_id,
                    updateUserDto.updated_by
                );

                dbContext.SaveChanges();
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID: {UserId}", user_id);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating user: {ex.Message}");
            }
        }

        // Inside UsersController.cs

        [HttpPatch]
        [Route("update_user/{user_id}")]
        public IActionResult PatchUser(string user_id, [FromBody] PatchUserDto patchUserDto)
        {
            try
            {
                _logger.LogInformation("Patching user with ID: {UserId}", user_id);

                dbContext.PatchUser(user_id, patchUserDto);

                var updatedUser = dbContext.Users.Find(user_id);
                if (updatedUser == null)
                {
                    _logger.LogWarning("No record found after patching with ID: {UserId}", user_id);
                    return NotFound("No user found after patching");
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching user with ID: {UserId}", user_id);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error patching user: {ex.Message}");
            }
        }




        [HttpDelete]
        [Route("{user_id}")]
        public IActionResult DeleteUser(string user_id)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID: {UserId}", user_id);

                dbContext.DeleteUser(user_id);
                return Ok("You have successfully deleted the record");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID: {UserId}", user_id);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting user: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("ByRoleId/{role_id}")]
        public ActionResult<IEnumerable<User>> GetByRole(string role_id)
        {
            try
            {
                _logger.LogInformation("Getting users by role ID: {RoleId}", role_id);

                var users = dbContext.GetUserByRole(role_id);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users by role ID: {RoleId}", role_id);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving users: {ex.Message}");
            }
        }
    }
}
