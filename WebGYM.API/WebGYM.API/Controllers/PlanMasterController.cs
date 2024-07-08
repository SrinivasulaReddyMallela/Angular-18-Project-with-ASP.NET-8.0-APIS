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
using WebGYM.Interface;
using WebGYM.Models;
using WebGYM.ViewModels;

namespace WebGYM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlanMasterController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlanMaster _planMaster;
        private readonly ILogger<PlanMasterController> _logger;
        public PlanMasterController(IPlanMaster planMaster, IMapper mapper, ILogger<PlanMasterController> logger)
        {
            _planMaster = planMaster;
            _mapper = mapper;
            _logger = logger;
        }
        // GET: api/PlanMaster
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _planMaster.GetPlanMasterList());
        }

        // GET: api/PlanMaster/5
        [HttpGet("{id}", Name = "GetPlan")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _planMaster.GetPlanMasterbyId(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        // POST: api/PlanMaster
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlanMasterViewModel planMasterViewModel)
        {
            try
            {
                if (await _planMaster.CheckPlanExits(planMasterViewModel.PlanName))
                {
                    return StatusCode(409);
                }
                else
                {
                    var userId = this.User.FindFirstValue(ClaimTypes.Name);
                    var tempplanMaster = _mapper.Map<PlanMaster>(planMasterViewModel);
                    tempplanMaster.CreateUserID = Convert.ToInt32(userId);
                    tempplanMaster.RecStatus = true;
                    await _planMaster.InsertPlan(tempplanMaster);

                    return Ok(new { Message = "Created Plan" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        // PUT: api/PlanMaster/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PlanMasterViewModel planMasterViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = this.User.FindFirstValue(ClaimTypes.Name);
                    var tempplanMaster = _mapper.Map<PlanMaster>(planMasterViewModel);
                    tempplanMaster.CreateUserID = Convert.ToInt32(userId);
                    tempplanMaster.RecStatus = true;
                    var result = await _planMaster.UpdatePlanMaster(tempplanMaster);
                    if (result)
                        return Ok(new { Message = "Updated Plan" });
                    else
                        return NotFound();
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await _planMaster.DeletePlan(id))
                    return Ok(new { Message = "Deleted Plan" });
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
