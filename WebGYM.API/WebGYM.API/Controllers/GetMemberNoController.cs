using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebGYM.Interface;
using WebGYM.ViewModels;

namespace WebGYM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GetMemberNoController : ControllerBase
    {
        private readonly IMemberRegistration _memberRegistration;
        private readonly ILogger<GetMemberNoController> _logger;
        public GetMemberNoController(IMemberRegistration memberRegistration, ILogger<GetMemberNoController> logger)
        {
            _memberRegistration = memberRegistration;
            _logger = logger;
        }


        // POST: api/GetMemberNo
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MemberRequest memberRequest)
        {
            try
            {
                return Ok(await _memberRegistration.GetMemberNoList(memberRequest.MemberName, Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.Name))));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

    }
}
