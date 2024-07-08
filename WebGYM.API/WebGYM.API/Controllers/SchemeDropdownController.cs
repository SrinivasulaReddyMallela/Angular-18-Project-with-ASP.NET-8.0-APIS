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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SchemeDropdownController : ControllerBase
    {

        private readonly ISchemeMaster _schemeMaster;
        private readonly ILogger<SchemeDropdownController> _logger;
        public SchemeDropdownController(ISchemeMaster schemeMaster, ILogger<SchemeDropdownController> logger)
        {
            _schemeMaster = schemeMaster;
            _logger = logger;
        }

        // GET: api/SchemeDropdown
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _schemeMaster.GetActiveSchemeMasterList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }
    }
}
