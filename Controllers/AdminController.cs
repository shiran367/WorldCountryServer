using Microsoft.AspNetCore.Mvc;
using WorldCountry.BL;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldCountry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        // GET: api/<AdminController>
        [HttpGet("GetAllUsers")]
        public List<User> GetAllUser()
        {
            User user = new User();
            return user.AllUsers();
        }

        // GET api/<AdminController>/5
        [HttpGet("GetData")]
        public AdminDashboardStats GetData(DateTime fromDate, DateTime toDate)
        {
            AdminDashboardStats adminDashboard= new AdminDashboardStats();

            return adminDashboard.AllData(fromDate, toDate);
        }

        // POST api/<AdminController>
        [HttpPost("PostLocked")]
        public int PostLocked(int UserId)
        {
            User user = new User();

           return user.Locked(UserId);
        }

    
    }
}
