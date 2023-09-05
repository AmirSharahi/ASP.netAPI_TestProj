using EfCore6Test.Data;
using EfCore6Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EfCore6Test.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/")]
    public class UserController : Controller
    {
        public TestContext DbContext { get; set; }

        public UserController(TestContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(int userId)
        {
            try
            {
                User user = DbContext.Users.FirstOrDefault(u => u.UserId == userId);

                if (user == null)
                    return StatusCode((int)HttpStatusCode.NotFound);

                return StatusCode((int)HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("InsertUser")]
        public IActionResult InsertUser([FromBody] UserInsert userInsert)
        {
            try
            {
                if (userInsert == null)
                    return StatusCode((int)HttpStatusCode.BadRequest);

                if (DbContext.Users.Any(u => u.Username == userInsert.Username))
                    return StatusCode((int)HttpStatusCode.Conflict);

                User user = new(userInsert);

                var res = DbContext.Users.Add(user);
                DbContext.SaveChanges();
                
                return StatusCode((int)HttpStatusCode.OK, res.Entity.UserId);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        public IActionResult UpdateUser(int userId, [FromBody] User user)
        {
            try
            {
                if (!DbContext.Users.Any(u => u.UserId == userId))
                    return StatusCode((int)HttpStatusCode.NotFound);

                if (userId != user.UserId)
                    return StatusCode((int)HttpStatusCode.BadRequest);

                var res = DbContext.Users.Update(user);
                DbContext.SaveChanges();

                return StatusCode((int)HttpStatusCode.OK, res.Entity);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public IActionResult DeleteUser(int userId)
        {
            try
            {
                User user = DbContext.Users.FirstOrDefault(u => u.UserId == userId);

                if (user == null)
                    return StatusCode((int)HttpStatusCode.NotFound);

                var res = DbContext.Users.Remove(user);
                DbContext.SaveChanges();

                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}


//www.blyat.com/api/test/getuser?userid=10
//www.blyat.com/api/test/insertuser [Body]
//www.blyat.com/api/test/updateuser?userid=10 [Body]
//www.blyat.com/api/test/deleteuser?userid=10