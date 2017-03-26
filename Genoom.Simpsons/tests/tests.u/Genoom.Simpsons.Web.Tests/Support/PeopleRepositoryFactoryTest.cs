using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Genoom.Simpsons.Repository.Sql;
using Genoom.Simpsons.Repository.MongoDb;
using Genoom.Simpsons.Web.Support;

namespace Genoom.Simpsons.Web.Tests.Support
{
    [TestClass]
    public class PeopleRepositoryFactoryTest
    {
        // Public Methods
        /// <summary>
        /// Intention: Test that we can get a Sql Azure Repository
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void CreateAzureRepositoryOkTest()
        {
            //ARRANGE
            var testConfig = TestConfiguration();
            testConfig.GetSection("DbStrategy").Value = "Azure";

            //ACT
            var testData = PeopleRepositoryFactory.Create(testConfig);

            //ASSERT
            Assert.IsNotNull(testData);
            Assert.IsTrue(testData is PeopleRepositorySql);
        }

        /// <summary>
        /// Intention: Test that we can get a Sql Repository
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void CreateSqlRepositoryOkTest()
        {
            //ARRANGE
            var testConfig = TestConfiguration();
            testConfig.GetSection("DbStrategy").Value="Sql";

            //ACT
            var testData = PeopleRepositoryFactory.Create(testConfig);

            //ASSERT
            Assert.IsNotNull(testData);
            Assert.IsTrue(testData is PeopleRepositorySql);
        }

        /// <summary>
        /// Intention: Test that we can get a Sql Repository
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void CreateMongoDbRepositoryOkTest()
        {
            //ARRANGE
            var testConfig = TestConfiguration();
            testConfig.GetSection("DbStrategy").Value = "MongoDb";

            //ACT
            var testData = PeopleRepositoryFactory.Create(testConfig);

            //ASSERT
            Assert.IsNotNull(testData);
            Assert.IsTrue(testData is PeopleRepositoryMongoDb);
        }


        // Private Methods
        private IConfigurationRoot TestConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json");
            return configBuilder.Build();
        }
    }
}
