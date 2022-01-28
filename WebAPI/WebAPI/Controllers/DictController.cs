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
    public class DictController : ControllerBase
    {
        private readonly DictService service;
        public DictController(DictService _service)
        {
            service = _service;
        }

        [HttpGet]
        public async Task<IActionResult> queryDict([FromQuery] DictQueryCriteria criteria)
        {
            var filterBuilder = Builders<Dict>.Filter;
            var filter = filterBuilder.Empty;//.Eq(x => x.pid, criteria.pid);
            var dicts = await service.queryAll(filter);
            List<Dict> pageData;
            if (criteria.size != 0)
            {
                pageData = dicts.Skip((criteria.page) * criteria.size).Take(criteria.size).ToList();
            }
            else
            {
                pageData = dicts.ToList();
            }

            var totalRecords = dicts.Count();
            return Ok(new PagedResponse<List<DictViewModel>>(pageData.ToViewModel(), totalRecords));
        }

        [HttpPost]
        public async Task<Dict> Post([FromBody] DictViewModel viewModel)
        {
            var curr = viewModel.ToModel();
            curr.create_time = DateTime.Now;
            return await service.Create(curr);
        }

        [HttpPut]
        public async Task Put([FromBody] DictViewModel viewModel)
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
