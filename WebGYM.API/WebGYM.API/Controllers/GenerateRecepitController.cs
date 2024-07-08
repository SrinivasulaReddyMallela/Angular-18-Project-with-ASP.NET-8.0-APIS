using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebGYM.Interface;
using WebGYM.ViewModels;

namespace WebGYM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateRecepitController : ControllerBase
    {
        private readonly IGenerateRecepit _generateRecepit;
        private readonly ILogger<GenerateRecepitController> _logger;
        public GenerateRecepitController(IGenerateRecepit generateRecepit, ILogger<GenerateRecepitController> logger)
        {
            _generateRecepit = generateRecepit;
            _logger = logger;
        }


        // POST: api/GenerateRecepit
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GenerateRecepitRequestModel generateRecepitRequestModel)
        {
            try
            {
                return Ok(await _generateRecepit.Generate(generateRecepitRequestModel.PaymentId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

    }
}
