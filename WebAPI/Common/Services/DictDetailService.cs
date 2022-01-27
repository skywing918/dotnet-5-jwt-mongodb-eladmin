﻿namespace WebAPI.Common.Services
{
    using Microsoft.Extensions.Configuration;
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
            return curr.dictDetails.AsQueryable();
        }

        public async Task<DictDetail> Create(DictDetail dept)
        {
            var result = await _client.AddRecord(collectionName, dept).ConfigureAwait(false);
            return result;
        }
    }
}
