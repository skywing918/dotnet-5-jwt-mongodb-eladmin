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
    public class DeptController : ControllerBase
    {
        private readonly DeptService service;
        public DeptController(DeptService _service)
        {
            service = _service;
        }
        [HttpGet]
        public IActionResult queryDept([FromQuery] DeptQueryCriteria criteria)
        {
            List<Dept> pageData;
            if (criteria.size != 0)
            {
                pageData = service.GetDepts().Skip((criteria.page) * criteria.size).Take(criteria.size).ToList();
            }
            else
            {
                pageData = service.GetDepts().Where(d=>d.pid==criteria.pid).ToList();
            }
            var totalRecords = service.GetDepts().Count();
            return Ok(new PagedResponse<List<DeptViewModel>>(pageData.ToViewModel(), totalRecords));
        }
    }
}
