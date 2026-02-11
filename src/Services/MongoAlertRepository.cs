using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FundNavTracker.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace FundNavTracker.Services
{
    public class MongoAlertRepository : IAlertRepository
    {
        private readonly IMongoCollection<NavAlert> _collection;

        public MongoAlertRepository(IConfiguration configuration)
        {
            var connectionString = configuration["MONGO_CONNECTION_URI"];
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Mongo connection string missing. Set MONGO_CONNECTION_URI.");
            }

            var databaseName = configuration["Alerting:DatabaseName"] ?? "FundMaster";
            var collectionName = configuration["Alerting:CollectionName"] ?? "NavAlerts";

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<NavAlert>(collectionName);
        }

        public Task AddAsync(NavAlert alert)
        {
            return _collection.InsertOneAsync(alert);
        }

        public async Task<List<NavAlert>> GetActiveAsync()
        {
            return await _collection.Find(a => a.IsActive).ToListAsync();
        }

        public Task UpdateAsync(NavAlert alert)
        {
            return _collection.ReplaceOneAsync(a => a.Id == alert.Id, alert);
        }
    }
}
