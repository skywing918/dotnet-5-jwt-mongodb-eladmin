namespace WebAPI.Common.Services
{
    using Microsoft.Extensions.Configuration;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WebAPI.Common.Helper;
    using WebAPI.Common.Models;
    public class DeptService
    {
        private static readonly string collectionName = "depts";
        private readonly MongoDbHelper _client;

        public DeptService(IConfiguration configuation)
        {
            _client = new MongoDbHelper(configuation.GetConnectionString("EladminDb"));
        }


        public async Task<IQueryable<Dept>> queryAll(FilterDefinition<Dept> filter)
        {
            var results = await _client.GetWithFilter(collectionName, filter).ConfigureAwait(false);
            return results.AsQueryable();
        }

        public async Task<Dept> findById(string id)
        {
            var result = await _client.GetRecordById <Dept>(collectionName, dept=>dept.Id,id).ConfigureAwait(false);
            return result;
        }

        public async Task<List<Dept>>getSuperior(Dept deptDto, List<Dept> depts)
        {
            if (deptDto.pid == null)
            {
                var found = await findByPid(null);
                depts.AddRange(found);
                return depts;
            }
            depts.AddRange(await findByPid(deptDto.pid));
            var databyId = await findById(deptDto.pid);
            return await getSuperior(databyId, depts);
        }

        public async Task<IQueryable<Dept>> findByPid(string pid)
        {
            var filterBuilder = Builders<Dept>.Filter;
            var filter = filterBuilder.Eq(x => x.pid, pid);
            var results = await _client.GetWithFilter(collectionName, filter).ConfigureAwait(false);
            return results.AsQueryable();
        }

        public async Task<IQueryable<Dept>> findByRoleId(Guid roleId)
        {
            var filterBuilder = Builders<Dept>.Filter;
            var filter = filterBuilder.AnyEq(x => x.roleIds, roleId);
            var results = await _client.GetWithFilter(collectionName, filter).ConfigureAwait(false);
            return results.AsQueryable();
        }

        public async Task<Dept> Create(Dept dept)
        {
            var result = await _client.AddRecord(collectionName, dept).ConfigureAwait(false);
            return result;
        }

        public async Task Update(string id, Dept curr)
        {
            curr.Id = id;
            await _client.UpdateRecord(collectionName, dept => dept.Id, id, curr).ConfigureAwait(false);
        }

        public async Task Delete(List<string> ids)
        {
            foreach (var id in ids)
            {
                await _client.DeleteRecord<Dept>(collectionName, curr => curr.Id, id).ConfigureAwait(false);
            }
        }
    }
}
