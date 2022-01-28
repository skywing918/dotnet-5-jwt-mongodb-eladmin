using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            var viewModel = menuDtoList.ToVoViewModel(c => c.Id, c => c.pid);
            return viewModel;
        }

        // GET api/menus/lazy
        [HttpGet("lazy")]
        public async Task<IEnumerable<MenuViewModel>> queryAllMenu(string pid)
        {
            if (pid.Equals("0"))
            {
                pid = null;
            }
            var menus = await service.GetMenus(pid);
            var viewModel = menus.ToList().ToViewModel();
            return viewModel;
        }

        // GET api/menus/child
        [HttpGet("child")]
        public async Task<IEnumerable<string>> childMenu(string id)
        {
           var menuSet = new List<Menu>();
            var curr = await service.FindOne(id);
            menuSet.Add(curr);
            var menuList = await service.GetMenus(id);
            menuSet.Add(curr);
            menuSet = (await service.getChildMenus(menuList, menuSet)).ToList();
            var viewModel = menuSet.Select(m => m.Id).ToList();
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

        [HttpPost("superior")]
        public async Task<IActionResult> getMenuSuperior([FromBody] List<string> ids)
        {
            if (ids.Count() == 0)
            {
                return Ok(service.GetMenus(null));
            }
            var menuDtos = new List<Menu>();
            foreach(var id in ids)
            {
                var menuDto = await service.findById(id);
                var menus = await service.getSuperior(menuDto, new List<Menu>());
                menuDtos.AddRange(menus);
            }            

            return Ok(new PagedResponse<List<MenuViewModel>>(menuDtos.ToViewModel(), menuDtos.Count()));
        }

        [HttpPost]
        public async Task<Menu> Post([FromBody] MenuViewModel viewModel)
        {
            var curr = viewModel.ToModel();
            curr.create_time = DateTime.Now;
            return await service.Create(curr);
        }

        [HttpPut]
        public async Task Put([FromBody] MenuViewModel viewModel)
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
