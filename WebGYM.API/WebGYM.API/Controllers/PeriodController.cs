using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebGYM.Interface;
using WebGYM.Models;

namespace WebGYM.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PeriodController : ControllerBase
    {
        private readonly IPeriodMaster _periodMaster;
        private readonly ILogger<PeriodController> _logger;
        public PeriodController(IPeriodMaster periodMaster,ILogger<PeriodController> logger)
        {
            _periodMaster = periodMaster;
            _logger = logger;
        }

        // GET: api/Period
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _periodMaster.ListofPeriod());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

    }
}
