
namespace WebAPI.Common.Helper
{
    using Microsoft.Extensions.Configuration;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class MongoDbHelper
    {
        private static IMongoDatabase mdb;
        private string connectionString;

        public MongoDbHelper(string connectionStr)
        {
            connectionString = connectionStr;            
            var client = new MongoClient(connectionString);
            string dbName = MongoUrl.Create(connectionString).DatabaseName;

            mdb = client.GetDatabase(dbName);
        }

        private IMongoCollection<T> GetCollection<T>(string name) where T : class
        {
            return mdb.GetCollection<T>(name);
        }

        public async Task<IEnumerable<T>> GetAllList<T>(string collectionName) where T : class
        {
            var results = await GetCollection<T>(collectionName).FindAsync("{}").ConfigureAwait(false);
            return results.ToList();
        }

        public async Task<IEnumerable<T>> GetWithFilter<T>(string collectionName, FilterDefinition<T> filter) where T : class
        {
            var results = await GetCollection<T>(collectionName).FindAsync(filter).ConfigureAwait(false);
            return results.ToList();
        }

        public async Task<T> AddRecord<T>(string collectionName, T data) where T : class
        {
            await GetCollection<T>(collectionName).InsertOneAsync(data).ConfigureAwait(false);
            return data;
        }

        public async Task<T> GetRecordById<T>(string collectionName, Expression<Func<T, string>> keyField, string id) where T : class
        {
            var results = await GetCollection<T>(collectionName).FindAsync(Builders<T>.Filter.Eq(keyField, id));
            return results.FirstOrDefault();
        }

        public async Task UpdateRecord<T>(string collectionName, Expression<Func<T, string>> keyField, string id, T data) where T : class
        {
            var filter = Builders<T>.Filter.Eq(keyField, id);
            await GetCollection<T>(collectionName).ReplaceOneAsync(filter, data).ConfigureAwait(false);
        }

        public async Task DeleteRecord<T>(string collectionName, Expression<Func<T, string>> keyField, string id) where T : class
        {
            var filter = Builders<T>.Filter.Eq(keyField, id);
            await GetCollection<T>(collectionName).DeleteOneAsync(filter);
        }
    }
}
