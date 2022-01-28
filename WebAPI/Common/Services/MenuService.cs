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

        public MenuService(IConfiguration configuation, RoleService roleService)
        {
            _roleService = roleService;
            _client = new MongoDbHelper(configuation.GetConnectionString("EladminDb"));


        }
        public async Task<IEnumerable<Menu>> GetAll()
        {
            return await _client.GetAllList<Menu>(collectionName).ConfigureAwait(false);
        }
        public async Task<IEnumerable<Menu>> FindByRoles(IEnumerable<string> roleIds)
        {
            var roles = await _roleService.findById(roleIds);
            var menuIds = roles.SelectMany(r => r.MenuIds.ToList());
            var filterBuilder = Builders<Menu>.Filter;
            var filter = filterBuilder.In(x => x.Id, menuIds);
            return await _client.GetWithFilter(collectionName, filter).ConfigureAwait(false);
        }

        public async Task<Menu> findById(string id)
        {
            var curr = await _client.GetRecordById<Menu>(collectionName, obj => obj.Id, id).ConfigureAwait(false);

            return curr;
        }
        public async Task<List<Menu>> getSuperior(Menu menuDto, List<Menu> menus)
        {
            if (menuDto.pid == null)
            {
                var found = await findByPid(null);
                menus.AddRange(found);
                return menus;
            }
            menus.AddRange(await findByPid(menuDto.pid));
            var databyId = await findById(menuDto.pid);
            return await getSuperior(databyId, menus);
        }

        public async Task<IQueryable<Menu>> findByPid(string pid)
        {
            var filterBuilder = Builders<Menu>.Filter;
            var filter = filterBuilder.Eq(x => x.pid, pid);
            var results = await _client.GetWithFilter(collectionName, filter).ConfigureAwait(false);
            return results.AsQueryable();
        }

        public async Task<IQueryable<Menu>> GetMenusByCriteria(FilterDefinition<Menu> filter)
        {

            var results = await _client.GetWithFilter(collectionName, filter).ConfigureAwait(false);
            return results.AsQueryable();
        }

        public async Task<Menu> Create(Menu menu)
        {
            var result = await _client.AddRecord(collectionName, menu).ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<Menu>> getChildMenus(IEnumerable<Menu> menuList, List<Menu> menuSet)
        {
            foreach (var menu in menuList)
            {
                menuSet.Add(menu);
                var filterBuilder = Builders<Menu>.Filter;
                var filter = filterBuilder.Eq(x => x.pid, menu.Id);
                var menus = await _client.GetWithFilter(collectionName, filter).ConfigureAwait(false);
                if (menus.Count() != 0)
                {
                    await getChildMenus(menus, menuSet);
                }
            }

            return menuSet.AsQueryable();
        }

        public async Task<IQueryable<Menu>> GetMenus(string? pid)
        {
            var filterBuilder = Builders<Menu>.Filter;
            var filter = filterBuilder.Eq(x => x.pid, pid);
            var menus = await _client.GetWithFilter(collectionName, filter).ConfigureAwait(false);

            return menus.AsQueryable();
        }

        public async Task<Menu> FindOne(string id)
        {
            return await _client.GetRecordById<Menu>(collectionName, menu => menu.Id, id).ConfigureAwait(false);
        }

        public async Task Update(string id, Menu curr)
        {
            curr.Id = id;
            await _client.UpdateRecord(collectionName, obj => obj.Id, id, curr).ConfigureAwait(false);
        }

        public async Task Delete(List<string> ids)
        {
            foreach (var id in ids)
            {
                await _client.DeleteRecord<Menu>(collectionName, curr => curr.Id, id).ConfigureAwait(false);
            }
        }
    }
}
