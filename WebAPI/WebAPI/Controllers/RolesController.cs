using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Common.Services;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleService service;
        public RolesController(RoleService _service)
        {
            service = _service;
        }

        [HttpGet]
        public IActionResult queryRole([FromQuery] RoleQueryCriteria criteria)
        {
            var pageData = service.GetRoles().Skip((criteria.page) * criteria.size).Take(criteria.size).ToList();
            var totalRecords = service.GetRoles().Count();
            return Ok(new PagedResponse<List<RoleViewModel>>(pageData.ToViewModel(), totalRecords));
        }

        // GET api/roles/level
        [HttpGet("level")]
        public async Task<IActionResult> Get()
        {
            var result = new { level = 1 };
            return new OkObjectResult(JsonConvert.SerializeObject(result));
        }
    }
}
