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
    /// <summary>
    /// View All Active Plan Master Related API EndPoints
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AllActivePlanMasterController : ControllerBase
    {

        private readonly IPlanMaster _planMaster;
        private readonly ILogger<AllActivePlanMasterController> _logger;
        public AllActivePlanMasterController(IPlanMaster planMaster, ILogger<AllActivePlanMasterController> logger)
        {
            _planMaster = planMaster;
            _logger = logger;
        }

        //
        /// <summary>
        /// Get All Active Plan or  Get Specific Active Plan by ID
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        ///  GET: api/AllActivePlanMaster/5
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetAllActivePlan")]
        public async Task<IActionResult> Get(int? id)
        {
            try
            {
                if (id != null)
                {
                    return Ok(await _planMaster.GetActivePlanMasterList(id));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }
    }
}
