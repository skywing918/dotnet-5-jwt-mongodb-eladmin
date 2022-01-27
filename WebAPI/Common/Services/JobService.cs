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
    public class JobService
    {
        private static readonly string collectionName = "jobs";
        private readonly MongoDbHelper _client;

        public JobService(IConfiguration configuation)
        {
            _client = new MongoDbHelper(configuation.GetConnectionString("EladminDb"));
        }

        public async Task<IQueryable<Job>> queryAll(FilterDefinition<Job> filter)
        {
            var results = await _client.GetWithFilter(collectionName, filter).ConfigureAwait(false);
            return results.AsQueryable();
        }

        public async Task<Job> Create(Job curr)
        {
            var result = await _client.AddRecord(collectionName, curr).ConfigureAwait(false);
            return result;
        }
    }
}
