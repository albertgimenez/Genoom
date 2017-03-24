using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genoom.Simpsons.Repository.Sql.Tests
{
    [TestClass]
    public class PeopleRepositorySqlTest
    {
        private static string Connectionstring { get; set; }



        [ClassInitialize]
        public static void ClassInitialize()
        {
            GetTestConnectionString();
        }



        /// <summary>
        /// Intention: Get Homer Simpson.
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetPersonAsyncHomerOkTest()
        {
            //ARRANGE
            var testObject = new PeopleRepositorySql(Connectionstring);

            //ACT
            var testData = await testObject.GetPersonAsync(Guid.Parse("cb840eba-90bf-4d1f-8d3c-3b803f265959")); //Homer Simpson

            //ASSERT
            Assert.IsNotNull(testData);
            Assert.AreEqual("Homer", testData.Name);
            Assert.AreEqual("Simpson", testData.Lastname);
        }

        /// <summary>
        /// Intention: Get the family tree for Homer Simpson, and check he has the correct family members in all the tree directions.
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetFamilyAsyncHomerOkTest()
        {
            //ARRANGE
            var testObject = new PeopleRepositorySql(Connectionstring);

            //ACT
            var testData = await testObject.GetFamilyAsync(Guid.Parse("cb840eba-90bf-4d1f-8d3c-3b803f265959")); //Homer Simpson

            //ASSERT
            Assert.IsNotNull(testData);
            Assert.IsTrue(testData.Any(p => p.Name == "Marge" && p.Lastname == "Bouvier" && p.Relationship == Model.RelationshipEnum.Partner));
            Assert.IsTrue(testData.Count(p => p.Relationship == Model.RelationshipEnum.Child) == 3); // Bart, Lisa, Maggie
            Assert.IsTrue(testData.Any(p => p.Name == "Abraham" && p.Lastname == "Simpson" && p.Relationship == Model.RelationshipEnum.Parent));
            Assert.IsTrue(testData.Any(p => p.Name == "Penelope" && p.Lastname == "Olsen" && p.Relationship == Model.RelationshipEnum.Parent));
        }

        [TestMethod]
        public async Task GetChildrenAsyncBasicOkTest()
        {
            //ARRANGE

            //ACT

            //ASSERT
        }

        [TestMethod]
        public async Task GetTreeAsyncBasicOkTest()
        {
            //ARRANGE

            //ACT

            //ASSERT
        }

        [TestMethod]
        public async Task HasPartnerAsyncBasicOkTest()
        {
            //ARRANGE

            //ACT

            //ASSERT
        }

        [TestMethod]
        public async Task AddChildAsyncBasicOkTest()
        {
            //ARRANGE

            //ACT

            //ASSERT
        }



        private static void GetTestConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            Connectionstring = configuration.GetConnectionString("SqlConnectionTest");
        }
    }
}
