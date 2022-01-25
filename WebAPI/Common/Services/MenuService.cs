namespace WebAPI.Common.Services
{
    using Microsoft.Extensions.Configuration;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebAPI.Common.Helper;
    using WebAPI.Common.Models;
    using WebAPI.Common.Models.Enum;

    public class MenuService
    {
        private static readonly string collectionName = "menus";
        private readonly RoleService _roleService;
        private readonly MongoDbHelper _client;

        public MenuService(IConfiguration configuation,RoleService roleService)
        {
            _roleService = roleService;
            _client = new MongoDbHelper(configuation.GetConnectionString("EladminDb"));


        }
        public async Task<IEnumerable<Menu>> FindByRoles(IEnumerable<string> roleIds)
        {
            //await InitialData();

            var roles = await _roleService.findById(roleIds);
            var menuIds = roles.SelectMany(r => r.MenuIds.ToList());
            var filterBuilder = Builders<Menu>.Filter;
            var filter = filterBuilder.In(x => x.Id, menuIds);
            return await _client.GetWithFilter(collectionName, filter).ConfigureAwait(false);
        }

        public async Task<Menu> Create(Menu menu)
        {
            var result = await _client.AddRecord(collectionName, menu).ConfigureAwait(false);
            return result;
        }

        public async Task InitialData()
        {
            var root =
                new Menu
                {
                    //Id="1",
                    pid = null,
                    sub_count = 7,
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

            var map = new List<Menu>
            {
                new Menu                {

                    sub_count=3,
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
                new Menu                {

                    sub_count=3,
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
            };

            var rootData = await _client.AddRecord(collectionName, root).ConfigureAwait(false);
            foreach (var curr in map)
            {
                curr.pid = rootData.Id;
                await _client.AddRecord(collectionName, curr).ConfigureAwait(false);
            }
        }
    }
}
