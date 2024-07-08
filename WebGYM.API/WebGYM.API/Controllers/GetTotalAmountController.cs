using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class GetTotalAmountController : ControllerBase
    {
        private readonly IPlanMaster _planMaster;
        private readonly ILogger<GetTotalAmountController> _logger;
        public GetTotalAmountController(IPlanMaster planMaster, ILogger<GetTotalAmountController> logger)
        {
            _planMaster = planMaster;
            _logger = logger;
        }
        // POST: api/GetTotalAmount
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AmountRequestViewModel amountRequest)
        {
            try
            {
                if (ModelState.IsValid)
                    return Ok(await _planMaster.GetAmount(amountRequest.PlanId, amountRequest.SchemeId));
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
