using KisanGuru.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KisanGuru.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public RolesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //When we want to get the all the role details

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var allRoles = dbContext.Roles.ToList();
            return Ok(allRoles);
        }
    }
}
