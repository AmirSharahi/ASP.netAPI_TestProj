using System.Collections;
using EfCore6Test.Data;
using EfCore6Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Collections.Generic;
using System.Data;

namespace EfCore6Test.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/")]
    public class RuleController : Controller
    {
        private TestContext _dbContext { get; set; }

        public RuleController(TestContext DbContext)
        {
            _dbContext = DbContext;
        }

        [HttpGet]
        public IActionResult GetRole(int? roleId)
        {
            try
            {
                if (roleId == null)
                {
                    List<Role> roles = _dbContext.Roles.ToList();

                    if (!roles.Any())
                        return StatusCode((int)HttpStatusCode.NotFound);

                    return StatusCode((int)HttpStatusCode.OK , roles);
                }

                Role role = _dbContext.Roles.FirstOrDefault(r => r.RoleId == roleId);

                if (role == null)
                    return StatusCode((int)HttpStatusCode.NotFound);

                return StatusCode((int)HttpStatusCode.OK, role);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public IActionResult InsertRole(string roleTitle)
        {
            try
            {
                if (roleTitle == null)
                    return StatusCode((int)HttpStatusCode.BadRequest);
                
                Role role = _dbContext.Roles.FirstOrDefault(r => r.RoleTitle == roleTitle);
                if (role != null)
                    return StatusCode((int)HttpStatusCode.Conflict);

                role = new(roleTitle);

                var res = _dbContext.Roles.Add(role);
                _dbContext.SaveChanges();

                return StatusCode((int)HttpStatusCode.OK, res.Entity.RoleId);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateRole(int roleId, [FromBody] Role role)
        {
            try
            {
                if (roleId != role.RoleId)
                    return StatusCode((int)HttpStatusCode.BadRequest);

                if (!_dbContext.Roles.Any(r => r.RoleId == roleId))
                    return StatusCode((int)HttpStatusCode.NotFound);

                var res = _dbContext.Roles.Update(role);
                _dbContext.SaveChanges();

                return StatusCode((int)HttpStatusCode.OK, res.Entity);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteRole(int roleId)
        {
            try
            {
                List<Role> roles = _dbContext.Roles.Where(r => r.RoleId == roleId).ToList();
                
                if (!roles.Any())
                    return StatusCode((int)HttpStatusCode.NotFound);
                
                for (int i = 0; i < roles.Count(); i++)
                    _dbContext.Roles.Remove(roles[i]);

                _dbContext.SaveChanges();

                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
