﻿namespace WebAPI.Common.Services
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
    public class DictService
    {
        private static readonly string collectionName = "dict";
        private readonly MongoDbHelper _client;
        public DictService(IConfiguration configuation)
        {
            _client = new MongoDbHelper(configuation.GetConnectionString("EladminDb"));
        }

        public async Task<IQueryable<Dict>> queryAll(FilterDefinition<Dict> filter)
        {
            var results = await _client.GetWithFilter(collectionName, filter).ConfigureAwait(false);
            return results.AsQueryable();
        }

        public async Task<Dict> Create(Dict dept)
        {
            var result = await _client.AddRecord(collectionName, dept).ConfigureAwait(false);
            return result;
        }

        public async Task Update(string id, Dict curr)
        {
            curr.Id = id;
            await _client.UpdateRecord(collectionName, job => job.Id, id, curr).ConfigureAwait(false);
        }

        public async Task Delete(List<string> ids)
        {
            foreach (var id in ids)
            {
                await _client.DeleteRecord<Dict>(collectionName, curr => curr.Id, id).ConfigureAwait(false);
            }
        }
    }
}
