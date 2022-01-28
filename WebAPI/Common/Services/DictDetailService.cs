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
    public class DictDetailService
    {
        private static readonly string collectionName = "dict";
        private readonly MongoDbHelper _client;
        public DictDetailService(IConfiguration configuation)
        {
            _client = new MongoDbHelper(configuation.GetConnectionString("EladminDb"));
        }

        public async Task<IQueryable<DictDetail>> queryAll(string dictName)
        {
            var curr = await _client.GetRecordById<Dict>(collectionName, dict => dict.name, dictName).ConfigureAwait(false);            
            return curr.dictDetails?.AsQueryable();
        }

        public async Task Create(string dictId, DictDetail detail)
        {
            var dict = await _client.GetRecordById<Dict>(collectionName, job => job.Id, dictId).ConfigureAwait(false);
            if (dict.dictDetails == null)
            {
                dict.dictDetails = new List<DictDetail>();
            }
            dict.dictDetails.Add(detail);
            await _client.UpdateRecord(collectionName, curr => curr.Id, dictId, dict).ConfigureAwait(false);
        }

        public async Task Update(string dictId, DictDetail curr)
        {
            var dict = await _client.GetRecordById<Dict>(collectionName, obj => obj.Id, dictId).ConfigureAwait(false);
            dict.dictDetails = dict.dictDetails.Select(d =>
            {
                if (d.Id == curr.Id)
                {
                    return curr;
                }
                return d;
            }).ToList();
            await _client.UpdateRecord(collectionName, obj => obj.Id, dictId, dict).ConfigureAwait(false);
        }

        public async Task Delete(string id)
        {
            var filterBuilder = Builders<Dict>.Filter;
            var filter = filterBuilder.ElemMatch(x => x.dictDetails,x=>x.Id== id);
            var dicts = await _client.GetWithFilter(collectionName, filter).ConfigureAwait(false);
            var working = dicts.FirstOrDefault();
            if(working != null)
            {
                working.dictDetails = working.dictDetails.Where(x => x.Id != id).ToList();
            }
            await _client.UpdateRecord(collectionName, obj => obj.Id, working.Id, working).ConfigureAwait(false);
        }
    }
}
