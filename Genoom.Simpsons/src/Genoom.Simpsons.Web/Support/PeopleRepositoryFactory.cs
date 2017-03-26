using System;
using Microsoft.Extensions.Configuration;
using Genoom.Simpsons.Repository;
using Genoom.Simpsons.Repository.MongoDb;
using Genoom.Simpsons.Repository.Sql;

namespace Genoom.Simpsons.Web.Support
{
    public static class PeopleRepositoryFactory
    {
        public static IPeopleRepository Create(IConfigurationRoot config)
        {
            var strategyName = config.GetSection("DbStrategy").Value;

            switch (strategyName.ToLower())
            {
                case "azure":
                    return new PeopleRepositorySql(config.GetConnectionString("SqlConnectionAzure"));
                case "sql":
                    return new PeopleRepositorySql(config.GetConnectionString("SqlConnectionLocal"));
                case "mongodb":
                    return new PeopleRepositoryMongoDb(
                        connectionString: config.GetConnectionString("MongoDbConnection"),
                        database: config.GetSection("MongoDbConfig").GetSection("Database").Value,
                        collectionName: config.GetSection("MongoDbConfig").GetSection("Collection").Value
                    );
                default:
                    throw new PlatformNotSupportedException();
            };
        }
    }
}
