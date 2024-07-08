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
using WebGYM.ViewModels;

namespace WebGYM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignRolesController : ControllerBase
    {
        private readonly IUsersInRoles _usersInRoles;
        private readonly ILogger<AssignRolesController> _logger;
        public AssignRolesController(IUsersInRoles usersInRoles, ILogger<AssignRolesController> logger)
        {
            _usersInRoles = usersInRoles;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _usersInRoles.GetAssignRoles());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }


        // POST: api/UsersInRoles
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsersInRoles usersInRoles)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _usersInRoles.CheckRoleExists(usersInRoles))
                        return StatusCode(409);
                    else
                    {
                        usersInRoles.UserRolesId = 0;
                        var result = await _usersInRoles.AssignRole(usersInRoles);
                        if (result)
                            return StatusCode(200);
                        else
                            return NotFound();
                    }
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
