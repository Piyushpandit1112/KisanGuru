using KisanGuru.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KisanGuru.Models.Entities;
using System.Collections.Generic;
using KisanGuru.Models;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
namespace KisanGuru.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public FarmersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //When we want to get the all the farmers details

        [HttpGet]
        public IActionResult GetAllFarmers(int page = 1, int pageSize = 10)
        {
            try
            {
                    var farmers = dbContext.Farmers
                                         .Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToList();
                    return Ok(farmers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");

            }
        }

        //Sometime we want single farmer information
        [HttpGet]
        [Route("{farmer_id}")]
        public IActionResult GetUserById(string farmer_id)
        {
            var farmer = dbContext.Farmers.Find(farmer_id);
            if (farmer is null)
            {
                return NotFound("You have entered wrong id");
            }
            return Ok(farmer);
        }


        //When we want to insert the farmer details

        [HttpPost]
        public IActionResult AddFarmer(AddFarmerDto addFarmerDto)
        {
            try
            {
                var farmerEntity = new Farmer()
                {
                    farmer_id=addFarmerDto.farmer_id,
                    user_id=addFarmerDto.user_id,
                    farm_size=addFarmerDto.farm_size,
                    farm_location=addFarmerDto.farm_location,
                    pin_code=addFarmerDto.pin_code,
                    irrigation_method=addFarmerDto.irrigation_method,
                    soil_type=addFarmerDto.soil_type,
                    farming_experience=addFarmerDto.farming_experience,
                    membership_status=addFarmerDto.membership_status,
                    language_preference=addFarmerDto.language_preference,
                    inserted_by=addFarmerDto.inserted_by,
                    updated_by=addFarmerDto.updated_by
                };

                dbContext.Farmers.Add(farmerEntity);
                dbContext.SaveChanges();
                return Ok(farmerEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding farmer details: {ex.Message}");
            }
        }

        // When we want to update our record in the table
        [HttpPut]
        [Route("{farmer_id}")]
        public IActionResult UpdateEmployee(string farmer_id, UpdateFarmerDto updateFarmerDto)
        {
            try
            {
                var farmer = dbContext.Farmers.Find(farmer_id);
                if (farmer == null)
                {
                    return NotFound("There is no record with this ID");
                }

                farmer.farm_size = updateFarmerDto.farm_size;
                farmer.farm_location=updateFarmerDto.farm_location;
                farmer.pin_code = updateFarmerDto.pin_code;
                farmer.irrigation_method = updateFarmerDto.irrigation_method;
                farmer.soil_type = updateFarmerDto.soil_type;
                farmer.farming_experience = updateFarmerDto?.farming_experience;
                farmer.membership_status = updateFarmerDto?.membership_status;
                farmer.language_preference = updateFarmerDto.language_preference;

                dbContext.SaveChanges();
                return Ok(farmer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating farmer: {ex.Message}");
            }
        }




        //When we want to delete the farmers

        //To delete the record
        [HttpDelete]
        [Route("{farmer_id}")]
        public IActionResult DeleteFarmer(string farmer_id)
        {
            try
            {
                var farmer = dbContext.Farmers.Find(farmer_id);
                if (farmer is null)
                {
                    return NotFound("No record found with this id");
                }
                dbContext.Farmers.Remove(farmer);
                dbContext.SaveChanges();
                return Ok("You have successfully deleted the record");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting user data: {ex.Message}");

            }

        }

        



    }
}
