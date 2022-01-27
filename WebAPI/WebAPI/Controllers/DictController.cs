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
            var menus = await service.queryAll(filter);
            List<Dict> pageData;
            if (criteria.size != 0)
            {
                pageData = menus.Skip((criteria.page) * criteria.size).Take(criteria.size).ToList();
            }
            else
            {
                pageData = menus.ToList();
            }

            var totalRecords = menus.Count();
            return Ok(new PagedResponse<List<DictViewModel>>(pageData.ToViewModel(), totalRecords));
        }
    }
}
