using System.Net;
using EfCore6Test.Data;
using EfCore6Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCore6Test.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/")]
    public class LoginController : Controller
    {
        public TestContext _dbContext { get; set; }
        public LoginController(TestContext db)
        {
            _dbContext = db;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public IActionResult login(string userName, string password)
        {
            if (userName == null && password == null)
                return StatusCode((int)HttpStatusCode.BadRequest);

            if (!_dbContext.Users.Any(u => u.Username == userName))
                return StatusCode((int)HttpStatusCode.NotFound);

            var userR = _dbContext.Users.Single(u => u.Username == userName);
            
            //var userR = _dbContext.Users.Include(u => u.Role).Single(u => u.Username == userName);

            if (userR.Password != password)
                return StatusCode((int)HttpStatusCode.NotFound);

            if (userR.Password == password)
            {
                userR.LastLoginTime = DateTime.Now;
                _dbContext.SaveChanges();
                return StatusCode((int)HttpStatusCode.OK , userR);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        //[HttpGet]
        //[Route("api/[controller]")]
        //public IActionResult ValidateUser(string userName, int otp)
        //{
        //    if (userName == null && otp == null)
        //        return StatusCode((int)HttpStatusCode.BadRequest);

        //    if (!_dbContext.Users.Any(u => u.Username == userName))
        //        return StatusCode((int)HttpStatusCode.NotFound);

        //    if (_dbContext.Users.Any(u => u.Username == userName) && otp == 93)
        //        return StatusCode((int)HttpStatusCode.OK);

        //    return StatusCode((int)HttpStatusCode.InternalServerError);
        //}

        [HttpPut]
        [Route("api/[controller]")]
        public IActionResult ResetPass(string userName, [FromBody] string pass)
        {
            try
            {
                if (userName == null && pass == null)
                    return StatusCode((int)HttpStatusCode.BadRequest);

                if (!_dbContext.Users.Any(u => u.Username == userName))
                    return StatusCode((int)HttpStatusCode.NotFound);

                _dbContext.Users.Single(u => u.Username == userName).Password = pass;
                _dbContext.SaveChanges();

                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
