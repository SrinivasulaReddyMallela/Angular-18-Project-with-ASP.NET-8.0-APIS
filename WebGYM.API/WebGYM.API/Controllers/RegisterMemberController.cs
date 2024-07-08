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
using Newtonsoft.Json;
using WebGYM.API.Common;
using WebGYM.Interface;
using WebGYM.Models;
using WebGYM.ViewModels;

namespace WebGYM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterMemberController : ControllerBase
    {
        private readonly IMemberRegistration _memberRegistration;
        private readonly IUrlHelper _urlHelper;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterMemberController> _logger;
        public RegisterMemberController(IUrlHelper urlHelper, IMemberRegistration memberRegistration, IMapper mapper, ILogger<RegisterMemberController> logger)
        {
            _memberRegistration = memberRegistration;
            _urlHelper = urlHelper;
            _mapper = mapper;
            _logger = logger;
        }
        // GET: api/RegisterMember
        [HttpGet(Name = "GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParameters)
        {
            try
            {
                var userId = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.Name));
                List<MemberRegistrationGridModel> allMembers = (await _memberRegistration.GetAll(queryParameters, userId)).ToList();
                var allItemCount = await _memberRegistration.Count(userId);
                var paginationMetadata = new
                {
                    totalCount = allItemCount,
                    pageSize = queryParameters.PageCount,
                    currentPage = queryParameters.Page,
                    totalPages = queryParameters.GetTotalPages(allItemCount)
                };
                Request.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
                return Ok(new
                {
                    value = allMembers
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        // GET: api/RegisterMember/5
        [HttpGet("{id}", Name = "GetMember")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _memberRegistration.GetMemberbyId(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }

        }

        // POST: api/RegisterMember
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MemberRegistrationViewModel member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!await _memberRegistration.CheckNameExits(member.MemberFName, member.MemberLName, member.MemberMName))
                    {
                        var userId = this.User.FindFirstValue(ClaimTypes.Name);
                        var automember = _mapper.Map<MemberRegistration>(member);
                        automember.JoiningDate = DateTime.Now;
                        automember.Createdby = Convert.ToInt32(userId);

                        var result = await _memberRegistration.InsertMember(automember);
                        if (result > 0)
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

        // PUT: api/RegisterMember/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MemberRegistrationViewModel member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var storedMemberid = await _memberRegistration.CheckNameExitsforUpdate(member.MemberFName, member.MemberLName,member.MemberMName);
                    if (storedMemberid == member.MemberId || storedMemberid == 0)
                    {
                        var automember = _mapper.Map<MemberRegistration>(member);
                        automember.JoiningDate = DateTime.Now;
                        var result = await _memberRegistration.UpdateMember(automember);
                        if (result > 0)
                            return Ok();
                        else
                            return NotFound();
                    }
                    else
                        return StatusCode(409);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _memberRegistration.DeleteMember(id);
                if (result)
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
