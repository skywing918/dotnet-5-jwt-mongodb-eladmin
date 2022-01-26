﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Common.Models;
using WebAPI.Common.Services;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly MenuService service;
        public MenusController(MenuService _service)
        {
            service = _service;
        }

        // GET api/menus/build
        [HttpGet("build")]
        public async Task<IEnumerable<MenuVoViewModel>> Get()
        {
            var roleIds = User.FindAll(ClaimTypes.Role)?.Select(r => r.Value);
            var menuDtoList = await service.FindByRoles(roleIds);

            var viewModel = menuDtoList.ToViewModel(c => c.Id, c => c.pid);
            return viewModel;
        }

        [HttpGet]
        public async Task<IActionResult> queryMenu([FromQuery] MenuQueryCriteria criteria)
        {
            var filterBuilder = Builders<Menu>.Filter;
            var filter = filterBuilder.Eq(x => x.pid, criteria.pid);
            var menus = await service.GetMenusByCriteria(filter);
            List<Menu> pageData;
            if (criteria.size != 0)
            {
                pageData = menus.Skip((criteria.page) * criteria.size).Take(criteria.size).ToList();
            }
            else
            {
                pageData = menus.ToList();
            }
               
            var totalRecords = menus.Count();
            return Ok(new PagedResponse<List<MenuViewModel>>(pageData.ToViewModel(), totalRecords));
        }
    }
}
