namespace WebAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebAPI.Common.Models;
    using WebAPI.Common.Models.Enum;
    using WebAPI.Common.Services;
    [Route("api/[controller]")]
    [ApiController]
    public class InitialController : ControllerBase
    {
        private readonly UserService userService;
        private readonly RoleService roleService;
        private readonly MenuService menuService;
        public InitialController(UserService _userService, RoleService _roleService, MenuService _menuService)
        {
            userService = _userService;
            roleService = _roleService;
            menuService = _menuService;
        }

        // POST api/user
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post()
        {

            await InitialMenuData();
            await InitialRoleData();
            await InitialUserData();
            return new OkObjectResult("Initial finished");
        }

        private async Task InitialMenuData()
        {
            var root =
                new Menu
                {
                    //Id="1",
                    pid = null,
                    sub_count = 6,
                    type = MenuType.Folder,
                    title = "系统管理",
                    name = null,
                    component = null,
                    menu_sort = 1,
                    icon = "system",
                    path = "system",
                    i_frame = false,
                    cache = false,
                    hidden = false,
                    permission = null,
                    create_by = null,
                    update_by = null,
                    create_time = DateTime.Now,
                };
            var rootData = await menuService.Create(root);
            var map = new List<Menu>
            {
                new Menu
                {
                    sub_count=0,
                    type = MenuType.Menu,
                    title = "用户管理",
                    name ="User",
                    component="system/user/index",
                    menu_sort=2,
                    icon="peoples",
                    path="user",
                    i_frame=false,
                    cache=false,
                    hidden=false,
                    permission="user:list",
                    create_by=null,
                    update_by=null,
                    create_time = DateTime.Now,
                },
                new Menu
                {
                    sub_count=0,
                    type = MenuType.Menu,
                    title = "角色管理",
                    name ="Role",
                    component="system/role/index",
                    menu_sort=3,
                    icon="role",
                    path="role",
                    i_frame=false,
                    cache=false,
                    hidden=false,
                    permission="roles:list",
                    create_by=null,
                    update_by=null,
                    create_time = DateTime.Now,
                },
                new Menu
                {

                    sub_count=0,
                    type = MenuType.Menu,
                    title = "菜单管理",
                    name ="Menu",
                    component="system/menu/index",
                    menu_sort=5,
                    icon="menu",
                    path="menu",
                    i_frame=false,
                    cache=false,
                    hidden=false,
                    permission="menu:list",
                    create_by=null,
                    update_by=null,
                    create_time = DateTime.Now,
                },
                 new Menu
                {

                    sub_count=0,
                    type = MenuType.Menu,
                    title = "部门管理",
                    name ="Dept",
                    component="system/dept/index",
                    menu_sort=6,
                    icon="dept",
                    path="dept",
                    i_frame=false,
                    cache=false,
                    hidden=false,
                    permission="dept:list",
                    create_by=null,
                    update_by=null,
                    create_time = DateTime.Now,
                } ,
                 new Menu
                {

                    sub_count=0,
                    type = MenuType.Menu,
                    title = "岗位管理",
                    name ="Job",
                    component="system/job/index",
                    menu_sort=7,
                    icon="Steve-Jobs",
                    path="job",
                    i_frame=false,
                    cache=false,
                    hidden=false,
                    permission="job:list",
                    create_by=null,
                    update_by=null,
                    create_time = DateTime.Now,
                },
                  new Menu
                {

                    sub_count=0,
                    type = MenuType.Menu,
                    title = "字典管理",
                    name ="Dict",
                    component="system/dict/index",
                    menu_sort=8,
                    icon="dictionary",
                    path="dict",
                    i_frame=false,
                    cache=false,
                    hidden=false,
                    permission="dict:list",
                    create_by=null,
                    update_by=null,
                    create_time = DateTime.Now,
                },
            };
            foreach (var curr in map)
            {
                curr.pid = rootData.Id;
                await menuService.Create(curr).ConfigureAwait(false);
            }
        }

        private async Task InitialRoleData()
        {
            var menus = await menuService.GetMenus(null);
            var role = new Role("超级管理员");
            role.MenuIds = menus.Select(m => m.Id).ToList();
            role.level = 1;
            role.dataScope = "全部";
            role.description = "-";
            role.update_by = "admin";
            role.create_time = DateTime.Now;
            role.update_time = DateTime.Now;
            await roleService.CreateAsync(role);
        }

        private async Task InitialUserData()
        {
            var roleId = roleService.GetRoles().FirstOrDefault()?.Id;
            var user = new User()
            {
                UserName = "admin",
                nick_name = "管理员",
                gender = "男",
                phone = "18888888888",
                Email = "201507802@qq.com",
                avatar_name = "avatar-20200806032259161.png",
                avatar_path = "/Users/jie/Documents/work/me/admin/eladmin/~/avatar/avatar-20200806032259161.png",
                Roles = new List<Guid> { roleId.Value },
                is_admin = true,
                enabled = true
            };
            var res = await userService.CreateAsync(user, "P@ssw0rd");            
        }

    }
}
