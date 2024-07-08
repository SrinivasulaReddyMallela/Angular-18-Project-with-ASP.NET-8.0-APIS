using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebGYM.Interface;
using WebGYM.Models;

namespace WebGYM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RemoveRoleController : ControllerBase
    {

        private readonly IUsersInRoles _usersInRoles;
        private readonly ILogger<RemoveRoleController> _logger;
        public RemoveRoleController(IUsersInRoles usersInRoles, ILogger<RemoveRoleController> logger)
        {
            _usersInRoles = usersInRoles;
            _logger = logger;
        }

        // POST: api/RemoveRole
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsersInRoles usersInRoles)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _usersInRoles.CheckRoleExists(usersInRoles))
                    {
                        usersInRoles.UserRolesId = 0;
                        if (await _usersInRoles.RemoveRole(usersInRoles))
                            return Ok();
                        else
                            return NotFound();
                    }
                    else
                        return StatusCode(409);
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }
    }
}
