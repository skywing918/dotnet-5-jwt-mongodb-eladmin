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

namespace WebAPI.ViewModels
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DictDetailController : ControllerBase
    {
        private readonly DictDetailService service;
        public DictDetailController(DictDetailService _service)
        {
            service = _service;
        }

        [HttpGet]
        public async Task<IActionResult> queryDictDetail([FromQuery] DictDetailQueryCriteria criteria)
        {
            var details = await service.queryAll(criteria.dictName);
            List<DictDetail> pageData;
            if (criteria.size != 0)
            {
                pageData = details.Skip((criteria.page) * criteria.size).Take(criteria.size).ToList();
            }
            else
            {
                pageData = details.ToList();
            }

            var totalRecords = details.Count();
            return Ok(new PagedResponse<List<DictDetailViewModel>>(pageData.ToViewModel(), totalRecords));
        }

       
    }
}
