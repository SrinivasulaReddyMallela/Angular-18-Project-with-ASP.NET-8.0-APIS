using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebGYM.API.Common;
using WebGYM.Interface;
using WebGYM.Models;
using WebGYM.ViewModels;

namespace WebGYM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsers _users;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        public UserController(IUsers users, IMapper mapper, ILogger<UserController> logger)
        {
            _users = users;
            _mapper = mapper;
            _logger = logger;
        }
        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _users.GetAllUsers());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "GetUsers")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _users.GetUsersbyId(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsersViewModel users)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _users.CheckUsersExits(users.UserName))
                        return StatusCode(409);
                    else
                    {
                        var userId = this.User.FindFirstValue(ClaimTypes.Name);
                        var tempUsers = _mapper.Map<Users>(users);
                        tempUsers.CreatedDate = DateTime.Now;
                        tempUsers.Createdby = Convert.ToInt32(userId);
                        tempUsers.Password = EncryptionLibrary.EncryptText(users.Password);
                        if (await _users.InsertUsers(tempUsers))
                            return Ok();
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


        // PUT: api/User/5
        [HttpPut("{id}")]
        #region Commented Code
        //public async Task<IActionResult> Put(int id, [FromBody] UsersViewModel users)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var tempUsers = _mapper.Map<Users>(users);
        //            tempUsers.CreatedDate = DateTime.Now;
        //            if (await _users.UpdateUsers(tempUsers))
        //                return Ok();
        //            else
        //                NotFound();
        //        }
        //        else
        //            return BadRequest(ModelState);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
        //        return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
        //    }
        //}
        #endregion

        public async Task<HttpResponseMessage> Put(int id, [FromBody] UsersViewModel users)
        {
            if (ModelState.IsValid)
            {
                var tempUsers = _mapper.Map<Users>(users);
                tempUsers.CreatedDate = DateTime.Now;
                await _users.UpdateUsers(tempUsers);
                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };

                return response;

            }
            else
            {
                var response = new HttpResponseMessage()
                {

                    StatusCode = HttpStatusCode.BadRequest
                };

                return response;
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                if (await _users.DeleteUsers(id))
                    return Ok();
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
