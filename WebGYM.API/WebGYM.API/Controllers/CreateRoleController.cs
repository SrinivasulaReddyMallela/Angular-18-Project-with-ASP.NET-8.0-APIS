using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
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
    public class CreateRoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRole _role;
        private readonly ILogger<CreateRoleController> _logger;
        public CreateRoleController(IMapper mapper, IRole role, ILogger<CreateRoleController> logger)
        {
            _mapper = mapper;
            _role = role;
            _logger = logger;
        }
        // GET: api/CreateRole
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _role.GetAllRole());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        // GET: api/CreateRole/5
        [HttpGet("{id}", Name = "GetRole")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _role.GetRolebyId(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        // POST: api/CreateRole
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoleViewModel roleViewModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if (await _role.CheckRoleExits(roleViewModel.RoleName))
                        return StatusCode(409);
                    else
                    {
                        var temprole = _mapper.Map<Role>(roleViewModel);
                        await _role.InsertRole(temprole);
                        return Ok(new { Message = "Role Created" });
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

        // PUT: api/CreateRole/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RoleViewModel roleViewModel)
        {
            try
            {
                var temprole = _mapper.Map<Role>(roleViewModel);
                var result = await _role.UpdateRole(temprole);
                if (result)
                    return Ok(new { Message = "Updated Role!" });
                else
                    return NotFound();
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _role.DeleteRole(id);
                if (result)
                    return Ok(new { Message = "Deleted Role" });
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }
    }
}
