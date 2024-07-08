using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public class RenewalController : ControllerBase
    {
        private readonly IRenewal _renewal;
        private readonly IPaymentDetails _paymentDetails;
        private readonly IPlanMaster _planMaster;
        private readonly ILogger<RenewalController> _logger;
        public RenewalController(IRenewal renewal, IPaymentDetails paymentDetails, IPlanMaster planMaster, ILogger<RenewalController> logger)
        {
            _renewal = renewal;
            _paymentDetails = paymentDetails;
            _planMaster = planMaster;
            _logger = logger;
        }
        // GET: api/Renewal/5
        [HttpGet("{memberNo}", Name = "GetRenewal")]
        public async Task<IActionResult> Get(string memberNo)
        {
            try
            {
                return Ok(await _renewal.GetMemberNo(memberNo, Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.Name))));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }
        // POST: api/Renewal
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RenewalViewModel renewalViewModel)
        {
            try
            {
                if (await _renewal.CheckRenewalPaymentExists(renewalViewModel.NewDate, renewalViewModel.MemberId))
                    return BadRequest(new { ReasonPhrase = "Already Renewed" });
                else
                {
                    int cmp = renewalViewModel.NewDate.CompareTo(renewalViewModel.NextRenwalDate);
                    var userId = this.User.FindFirstValue(ClaimTypes.Name);
                    if (cmp > 0)
                    {
                        var months = await _planMaster.GetPlanMonthbyPlanId(renewalViewModel.PlanID);
                        var calculatedNextRenewalDate = renewalViewModel.NewDate.AddMonths(months).AddDays(-1);
                        renewalViewModel.NextRenwalDate = calculatedNextRenewalDate;
                        renewalViewModel.Createdby = Convert.ToInt32(userId);
                        if (await _paymentDetails.RenewalPayment(renewalViewModel))
                            return Ok(new { ReasonPhrase = "Renewed Successfully" });
                        else
                            return BadRequest(new { ReasonPhrase = "Renewal Failed" });
                    }
                    if (cmp < 0)
                        return BadRequest(new { ReasonPhrase = "Invalid Date" });
                }
                return BadRequest(new { ReasonPhrase = "Something went wrong" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }
    }
}
