using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebGYM.API.Models;
using WebGYM.Interface;
using WebGYM.Models;

namespace WebGYM.API.Controllers
{
    public class HomeController  : Controller
    {
        private ISchemeMaster _schemeMaster;
        public HomeController(ISchemeMaster schemeMaster)
        {
            _schemeMaster = schemeMaster;
        }
         
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return Ok(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
