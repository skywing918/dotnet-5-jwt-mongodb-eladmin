using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class UsersController : ControllerBase
    {
        private readonly UserService service;
        public UsersController(UserService _service)
        {
            service = _service;
        }

        [HttpGet]
        public IActionResult queryUser([FromQuery]UserQueryCriteria criteria)
        {
            var pageData = service.GetUsers().Skip((criteria.page)*criteria.size).Take(criteria.size).ToList();
            var totalRecords = service.GetUsers().Count();
            return Ok(new PagedResponse<List<UserViewModel>>(pageData.ToViewModel(), totalRecords));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReqUserViewModel viewModel)
        {
            var curr = viewModel.ToModel();            
            var result = await service.CreateAsync(curr,"Password01!");
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPut]
        public async Task Put([FromBody] ReqUserViewModel viewModel)
        {
            var curr = viewModel.ToModel();
            curr.Id = viewModel.id.Value;            
            await service.Update(curr);
        }

        [HttpDelete]
        public async Task Delete(List<string> ids)
        {
            await service.Delete(ids);
        }
    }
}
