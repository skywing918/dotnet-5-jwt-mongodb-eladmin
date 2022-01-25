namespace WebAPI.Common.Services
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WebAPI.Common.Helper;
    using WebAPI.Common.Models;
    public class DeptService
    {
        private static readonly string collectionName = "Depts";
        private readonly MongoDbHelper _client;

        public DeptService(IConfiguration configuation)
        {
            _client = new MongoDbHelper(configuation.GetConnectionString("EladminDb"));
        }

        public IQueryable<Dept> GetDepts()
        {
            return new List<Dept>
            {
                new Dept
                {
                    Id="2",
                    pid="7",
                    name ="研发部",
                    enabled = true,
                    create_by=null,
                    update_by=null,
                    create_time = DateTime.Now,
                },
                new Dept
                {
                    Id="7",
                    pid=null,                    
                    name ="华南分部",
                    enabled = true,
                    create_by=null,
                    update_by=null,
                    create_time = DateTime.Now,
                },
            }.AsQueryable();
        }

        public async Task<Dept> Create(Dept dept)
        {
            var result = await _client.AddRecord(collectionName, dept).ConfigureAwait(false);
            return result;
        }
    }
}
