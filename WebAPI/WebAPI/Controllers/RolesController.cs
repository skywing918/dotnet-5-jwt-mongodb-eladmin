using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Common.Models;
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
            var pageData = service.GetRoles().Skip((criteria.page) * criteria.size).Take(criteria.size);
            var totalRecords = service.GetRoles().Count();
            return Ok(new PagedResponse<List<RoleViewModel>>(pageData.ToViewModel(), totalRecords));
        }

        // GET api/roles/all
        [HttpGet("all")]
        public async Task<IActionResult> queryAllRole()
        {
            var result = service.GetRoles();
            return new OkObjectResult(JsonConvert.SerializeObject(result.ToViewModel()));
        }

        // GET api/roles/level
        [HttpGet("level")]
        public async Task<IActionResult> getRoleLevel()
        {
            var result = new { level = 1 };
            return new OkObjectResult(JsonConvert.SerializeObject(result));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoleViewModel viewModel)
        {
            var curr = viewModel.ToModel();
            curr.create_time = DateTime.Now;
            curr.update_time = DateTime.Now;
            var result = await service.CreateAsync(curr);
            if (result.Succeeded)
            {
                return Ok();
            }            
            return BadRequest(result.Errors);
        }

        [HttpPut]
        public async Task Put([FromBody] RoleViewModel viewModel)
        {
            var curr = viewModel.ToModel();
            curr.Id = viewModel.id.Value;
            curr.update_time = DateTime.Now;
            await service.Update(curr);
        }

        [HttpDelete]
        public async Task Delete(List<string> ids)
        {
            await service.Delete(ids);
        }
    }
}
