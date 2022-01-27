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
    public class JobController : ControllerBase
    {
        private readonly JobService service;
        public JobController(JobService _service)
        {
            service = _service;
        }

        [HttpGet]
        public async Task<IActionResult> queryJob([FromQuery] JobQueryCriteria criteria)
        {
            var filterBuilder = Builders<Job>.Filter;
            var filter = filterBuilder.Empty;//.Eq(x => x.pid, criteria.pid);
            var menus = await service.queryAll(filter);
            List<Job> pageData;
            if (criteria.size != 0)
            {
                pageData = menus.Skip((criteria.page) * criteria.size).Take(criteria.size).ToList();
            }
            else
            {
                pageData = menus.ToList();
            }

            var totalRecords = menus.Count();
            return Ok(new PagedResponse<List<JobViewModel>>(pageData.ToViewModel(), totalRecords));
        }
    }
}
