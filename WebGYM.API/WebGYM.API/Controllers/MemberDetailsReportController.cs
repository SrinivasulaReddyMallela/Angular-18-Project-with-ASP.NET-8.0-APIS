using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MemberDetailsReportController : ControllerBase
    {
        private readonly IReports _reports;
        private readonly ILogger<MemberDetailsReportController> _logger;
        public MemberDetailsReportController(IReports reports, ILogger<MemberDetailsReportController> logger)
        {
            _reports = reports;
            _logger = logger;
        }

        // GET: api/MemberDetailsReport
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _reports.Generate_AllMemberDetailsReport());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }
    }
}
