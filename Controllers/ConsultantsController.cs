using KisanGuru.Data;
using KisanGuru.Models.Entities;
using KisanGuru.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KisanGuru.Controllers
{
    [Route("v1/api/kisan_mitra/consultant")]
    [ApiController]
    public class ConsultantsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ConsultantsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //When we want to get the all the consultants details

        [HttpGet]
        public IActionResult GetAllConsultants(int page = 1, int pageSize = 10)
        {
            try
            {
                var consultants = dbContext.Consultants
                                     .Skip((page - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToList();
                return Ok(consultants);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");

            }
        }

        //Sometime we want single consultants information
        [HttpGet]
        [Route("{consultant_id}")]
        public IActionResult GetConsultantById(string consultant_id)
        {
            var consultant = dbContext.Consultants.Find(consultant_id);
            if (consultant is null)
            {
                return NotFound("You have entered wrong id");
            }
            return Ok(consultant);
        }


        //When we want to insert the consultant details

        //public IActionResult AddConsultant(AddConsultantDto addConsultantDto)
        //{
        //    try
        //    {
        //        var consultantEntity = new Consultant()
        //        {
        //            user_id=addConsultantDto.user_id,
        //            consultant_id=addConsultantDto.consultant_id,
        //            expertise=addConsultantDto.expertise,
        //            experience=addConsultantDto.experience,
        //            subscription_status=addConsultantDto.subscription_status,
        //            subscription_expiry=addConsultantDto.subscription_expiry,
        //            inserted_by=addConsultantDto.inserted_by,
        //            updated_by=addConsultantDto.updated_by
        //        };

        //        dbContext.Consultants.Add(consultantEntity);
        //        dbContext.SaveChanges();
        //        return Ok(consultantEntity);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding consultant details: {ex.Message}");
        //    }
        //}

        [HttpPost]

        [Route("insert_consultant")]
        public IActionResult AddConsultant(AddConsultantDto addConsultantDto)
        {
            try
            {
                // Validate addConsultantDto here if necessary

                // Call InsertConsultant method from ApplicationDbContext
                dbContext.InsertConsultant(
                    addConsultantDto.user_id,
                    addConsultantDto.consultant_id,
                    addConsultantDto.expertise,
                    addConsultantDto.experience,
                    addConsultantDto.subscription_status,
                    addConsultantDto.subscription_expiry,
                    addConsultantDto.inserted_by,
                    addConsultantDto.updated_by
                );

                // Optionally retrieve the newly inserted consultant from DbContext
                var newConsultant = dbContext.Consultants.Find(addConsultantDto.consultant_id);

                return Ok(newConsultant); // Return the inserted consultant
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding consultant: {ex.Message}");
            }
        }





        // When we want to update our record in the table
        [HttpPut]
        [Route("{consultant_id}")]
        public IActionResult UpdateEmployee(string consultant_id, UpdateConsultantDto updateConsultantDto)
        {
            try
            {
                var consultant = dbContext.Consultants.Find(consultant_id);
                if (consultant == null)
                {
                    return NotFound("There is no record with this ID");
                }

                consultant.expertise = updateConsultantDto.expertise;
                consultant.experience=updateConsultantDto.experience;
                consultant.subscription_status=updateConsultantDto.subscription_status;
                consultant.subscription_expiry = updateConsultantDto.subscription_expiry;

                dbContext.SaveChanges();
                return Ok(consultant);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating consultant: {ex.Message}");
            }
        }


        [HttpPatch]
        [Route("update_consultant/{consultant_id}")]
        public IActionResult PatchConsultant(string consultant_id, PatchConsultantDto patchConsultantDto)
        {
            try
            {
                // Call PatchConsultant method from ApplicationDbContext
                dbContext.PatchConsultant(consultant_id, patchConsultantDto);

                // Optionally retrieve the updated consultant from DbContext
                var updatedConsultant = dbContext.Consultants.Find(consultant_id);

                return Ok(updatedConsultant); // Return the updated consultant
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error patching consultant: {ex.Message}");
            }
        }



        //When we want to delete the details of the consultant
        [HttpDelete]
        [Route("{consultant_id}")]
        public IActionResult DeleteConsultant(string consultant_id)
        {
            try
            {
                var consultant = dbContext.Consultants.Find(consultant_id);
                if (consultant is null)
                {
                    return NotFound("No record found with this id");
                }
                dbContext.Consultants.Remove(consultant);
                dbContext.SaveChanges();
                return Ok("You have successfully deleted the record");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting consultant data: {ex.Message}");

            }

        }
    }



}
