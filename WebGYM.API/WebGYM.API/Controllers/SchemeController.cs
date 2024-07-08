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
    public class SchemeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISchemeMaster _schemeMaster;
        private readonly ILogger<SchemeController> _logger;
        public SchemeController(ISchemeMaster schemeMaster, IMapper mapper, ILogger<SchemeController> logger)
        {
            _schemeMaster = schemeMaster;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Scheme
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _schemeMaster.GetSchemeMasterList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        // GET: api/Scheme/5
        [HttpGet("{id}", Name = "GetScheme")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _schemeMaster.GetSchemeMasterbyId(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        // POST: api/Scheme
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SchemeMasterViewModel schemeMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _schemeMaster.CheckSchemeNameExists(schemeMaster.SchemeName))
                        return StatusCode(409);
                    else
                    {
                        var userId = this.User.FindFirstValue(ClaimTypes.Name);
                        var tempSchemeMaster = _mapper.Map<SchemeMaster>(schemeMaster);
                        tempSchemeMaster.Createddate = DateTime.Now;
                        tempSchemeMaster.Createdby = Convert.ToInt32(userId);
                        if (await _schemeMaster.AddSchemeMaster(tempSchemeMaster))
                            return Ok();
                        else
                            return NotFound();
                    }
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new
                {
                    message = "An error occurred while creating the  crating Todo Item",
                    error = ex.Message
                });
            }
        }

        // PUT: api/Scheme/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SchemeMasterEditViewModel schemeMaster)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Convert.ToString(id)) || schemeMaster == null)
                    return BadRequest();
                else
                {
                    var temp = _mapper.Map<SchemeMaster>(schemeMaster);
                    if (await _schemeMaster.UpdateSchemeMaster(temp))
                        return Ok();
                    else
                        return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new
                {
                    message = "An error occurred while creating the  crating Todo Item",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await _schemeMaster.DeleteScheme(id))
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new
                {
                    message = "An error occurred while creating the  crating Todo Item",
                    error = ex.Message
                });
            }
        }
    }
}
