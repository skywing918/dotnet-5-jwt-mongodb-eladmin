using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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
    public class DeptController : ControllerBase
    {
        private readonly DeptService service;
        public DeptController(DeptService _service)
        {
            service = _service;
        }
        [HttpGet]
        public async Task<IActionResult> queryDept([FromQuery] DeptQueryCriteria criteria)
        {
            var filterBuilder = Builders<Dept>.Filter;
            var filter = filterBuilder.Eq(x => x.pid, criteria.pid);
            var depts = await service.queryAll(filter);
            List<Dept> pageData;
            if (criteria.size != 0)
            {
                pageData = depts.Skip((criteria.page) * criteria.size).Take(criteria.size).ToList();
            }
            else
            {
                pageData = depts.ToList();
            }

            var totalRecords = depts.Count();
            return Ok(new PagedResponse<List<DeptViewModel>>(pageData.ToViewModel(), totalRecords));
        }

        [HttpPost("superior")]
        public async Task<IActionResult> getDeptSuperior([FromBody] string id)
        {
            var deptDtos = new List<Dept>();

            var deptDto = await service.findById(id);
            var depts = await service.getSuperior(deptDto, new List<Dept>());
            deptDtos.AddRange(depts);

            return Ok(new PagedResponse<List<DeptViewModel>>(deptDtos.ToViewModel(), deptDtos.Count()));
        }

        [HttpPost]
        public async Task<Dept> Post([FromBody] DeptViewModel viewModel)
        {
            var curr = viewModel.ToModel();
            curr.create_time = DateTime.Now;
            return await service.Create(curr);
        }

        [HttpPut]
        public async Task Put([FromBody] DeptViewModel viewModel)
        {
            var curr = viewModel.ToModel();
            curr.update_time = DateTime.Now;
            await service.Update(viewModel.id, curr);
        }

        [HttpDelete]
        public async Task Delete(List<string> ids)
        {
            await service.Delete(ids);
        }
    }
}
