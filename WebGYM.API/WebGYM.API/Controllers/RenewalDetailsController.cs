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
    public class RenewalDetailsController : ControllerBase
    {
        private readonly IRenewal _renewal;
        private readonly ILogger<RenewalDetailsController> _logger;
        public RenewalDetailsController(IRenewal renewal, ILogger<RenewalDetailsController> logger)
        {
            _renewal = renewal;
            _logger = logger;
        }

        // POST: api/RenewalDetails
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MemberNoRequest memberNoRequest)
        {
            try
            {
                if (ModelState.IsValid)
                    return Ok(await _renewal.GetMemberNo(memberNoRequest.MemberNo, Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.Name))));
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
