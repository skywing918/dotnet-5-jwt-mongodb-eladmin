﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    }
}
