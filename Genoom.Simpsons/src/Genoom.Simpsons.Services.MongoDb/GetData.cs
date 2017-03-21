using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Genoom.Simpsons.Services.MongoDb
{
    public class GetData
    {
        public async Task<List<Model.Person>> GetPeople()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Genoom");
            var collection = database.GetCollection<Model.Person>("SimpsonsFamilyTree");

            return await collection.Find(x => x.Name == "Homer").ToListAsync();
        }
    }
}
