using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Genoom.Simpsons.Model;

namespace Genoom.Simpsons.Repository.MongoDb
{
    public class PeopleRepositoryMongoDb : IPeopleRepository
    {
        // Properties
        public string ConnectionString { get; }
        public string DatabaseName { get; }
        public string CollectionName { get; }

        // Ctor
        public PeopleRepositoryMongoDb(string connectionString, string database, string collectionName)
        {
            ConnectionString = connectionString;
            DatabaseName = database;
            CollectionName = collectionName;
        }

        // Public Methods
        public async Task<Person> GetPersonAsync(string id)
        {
            var collection = GetMongoDbCollection();

            return await collection.Find(x => x.Name == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PersonFamily>> GetFamilyAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PersonWithParents> GetTreeAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> HasPartnerAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> AddChildAsync(string parentId, Person child)
        {
            throw new NotImplementedException();
        }

        // Private Methods
        private IMongoCollection<Person> GetMongoDbCollection()
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DatabaseName);

            return database.GetCollection<Model.Person>(CollectionName);
        }
    }
}
