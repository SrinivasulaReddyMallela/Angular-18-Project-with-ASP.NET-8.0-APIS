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
    public class RenewalReportController : ControllerBase
    {
        private readonly IReports _reports;
        private readonly ILogger<RenewalReportController> _logger;
        public RenewalReportController(IReports reports, ILogger<RenewalReportController> logger)
        {
            _reports = reports;
            _logger = logger;
        }
      
        // POST: api/RenewalReport
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RenewalReportRequestModel value)
        {
            try
            {
                return Ok(await _reports.Get_RenewalReport(value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

    }
}
