using System.Net;
using EfCore6Test.Data;
using EfCore6Test.Models;
using Microsoft.AspNetCore.Mvc;

namespace EfCore6Test.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/")]
    public class LoginController : Controller
    {
        private TestContext _dbContext { get; set; }

        [HttpGet]
        [Route("api/[controller]")]
        public IActionResult login(string userName , [FromBody] string password)
        {
            try
            {
                if (userName == null && password == null)
                    return StatusCode((int)HttpStatusCode.BadRequest);

                if (!_dbContext.Users.Any(u => u.Username == userName))
                    return StatusCode((int)HttpStatusCode.NotFound);

                var user = _dbContext.Users.Single(u => u.Username == userName);
                var userRole = _dbContext.Roles.Single(r =>
                    r.Users.Single(u => u.Username == user.Username).Username == userName);

                if (user.Password != password)
                    return StatusCode((int)HttpStatusCode.NotFound);

                if (user.Password == password)
                {
                    user.LastLoginTime = DateTime.Now;
                    return StatusCode((int)HttpStatusCode.OK);
                }

                return StatusCode((int)HttpStatusCode.InternalServerError, new { user , userRole });
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/[controller]")]
        public IActionResult ResetPass(string userName, [FromBody] int otp)
        {
            try
            {
                if (userName == null && otp == null)
                    return StatusCode((int)HttpStatusCode.BadRequest);

                if (!_dbContext.Users.Any(u => u.Username == userName))
                    return StatusCode((int)HttpStatusCode.NotFound);
                
                if (_dbContext.Users.Any(u => u.Username == userName) && otp == 93)
                    _dbContext.Users.Update(_dbContext.Users.Single(u => u.Username == userName));

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }
    }
}
