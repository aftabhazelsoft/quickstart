using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("identity")]
    [Authorize]
    public class IdentityController: ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var claims = new JsonResult(from c in User.Claims select new {c.Type, c.Value});
            return Ok(new
            {
                message = "Hello From MVC Core API!", 
                claims
            });
        }
    }
}
