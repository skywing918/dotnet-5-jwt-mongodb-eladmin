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

        public async Task<IQueryable<Menu>> GetMenus()
        {
           var results = await _client.GetAllList<Menu>(collectionName);
           return results.AsQueryable();
        }

        public async Task<Menu> Create(Menu menu)
        {
            var result = await _client.AddRecord(collectionName, menu).ConfigureAwait(false);
            return result;
        }
    }
}
