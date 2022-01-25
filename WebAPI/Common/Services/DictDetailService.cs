namespace Common.Services
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WebAPI.Common.Helper;
    using WebAPI.Common.Models;
    class DictDetailService
    {
        private static readonly string collectionName = "DictDetail";
        private readonly MongoDbHelper _client;
        public DictDetailService(IConfiguration configuation)
        {
            _client = new MongoDbHelper(configuation.GetConnectionString("EladminDb"));
        }

        public IQueryable<DictDetail> GetDictDetails()
        {            
            return new List<DictDetail>
            {
                new DictDetail
                {
                    Id="1",
                    dict_id="1",
                    label ="激活",
                    value = "true",
                    create_by=null,
                    update_by=null,
                    create_time = DateTime.Now,
                },
                new DictDetail
                {
                    Id="2",
                    dict_id="1",
                    label ="禁用",
                    value = "false",
                    create_by=null,
                    update_by=null,
                    create_time = DateTime.Now,
                },
            }.AsQueryable();
        }

        public async Task<DictDetail> Create(DictDetail dept)
        {
            var result = await _client.AddRecord(collectionName, dept).ConfigureAwait(false);
            return result;
        }
    }
}
