using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IEnumerable<MenuViewModel>> Get()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var menuDtoList = await service.FindByUser(userId);

            var viewModel = menuDtoList.ToViewModel(c => c.Id, c => c.pid);
            return viewModel;
        }
    }
}
