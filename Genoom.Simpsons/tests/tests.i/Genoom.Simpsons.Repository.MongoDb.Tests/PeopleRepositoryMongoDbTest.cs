﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Genoom.Simpsons.Model;
using MongoDB.Driver;

namespace Genoom.Simpsons.Repository.MongoDb.Tests
{
    [TestClass]
    public class PeopleRepositoryMongoDbTest
    {
        // Properties
        private static string ConnectionstringTest { get; set; }
        private static string DatabaseTest { get; set; }
        private static string CollectionTest { get; set; }


        // Ctor
        [ClassInitialize]
        public static void ClassInitialize()
        {
            GetTestConnectionString();
            CleanDb();
            SeedDb();
        }


        // Public Methods
        /// <summary>
        /// Intention: Get Homer Simpson.
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetPersonAsyncOkTest()
        {
            //ARRANGE
            var testObject = new PeopleRepositoryMongoDb(ConnectionstringTest, DatabaseTest, CollectionTest);

            //ACT
            var testData = await testObject.GetPersonAsync("Homer");

            //ASSERT
            Assert.IsNotNull(testData);
            Assert.AreEqual("Homer", testData.Name);
            Assert.AreEqual("Simpson", testData.LastName);
        }

        /// <summary>
        /// Intention: Get the family tree for Homer Simpson, and check he has the correct family members in all the tree directions.
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetFamilyAsyncOkTest()
        {
            //ARRANGE
            var testObject = new PeopleRepositoryMongoDb(ConnectionstringTest, DatabaseTest, CollectionTest);

            //ACT
            var testData = await testObject.GetFamilyAsync("Homer");

            //ASSERT
            Assert.IsNotNull(testData);
            Assert.IsTrue(testData.Any(p => p.Name == "Marge" && p.LastName == "Bouvier" && p.Relationship == Model.RelationshipEnum.Partner));
            Assert.IsTrue(testData.Count(p => p.Relationship == Model.RelationshipEnum.Child) == 3); // Bart, Lisa, Maggie
            Assert.IsTrue(testData.Any(p => p.Name == "Abraham" && p.LastName == "Simpson" && p.Relationship == Model.RelationshipEnum.Parent));
            Assert.IsTrue(testData.Any(p => p.Name == "Penelope" && p.LastName == "Olsen" && p.Relationship == Model.RelationshipEnum.Parent));
        }

        /// <summary>
        /// Intention: Get the tree of Homer Simpson, we check that has Abraham, Penelope, Orville, Yuma as parents/parent of parents.
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetTreeAsyncOkTest()
        {
            //ARRANGE
            var testObject = new PeopleRepositoryMongoDb(ConnectionstringTest, DatabaseTest, CollectionTest);

            //ACT
            var testData = await testObject.GetTreeAsync("Homer");

            //ASSERT
            Assert.IsNotNull(testData);
            Assert.IsTrue(testData.Parents.Any(p => p.Name == "Abraham Simpson"));
            Assert.IsTrue(testData.Parents.Any(p => p.Name == "Penelope Olsen"));
            Assert.IsTrue(testData.Parents.SingleOrDefault(p => p.Name == "Abraham Simpson").Parents.Any(p => p.Name == "Orville Simpson"));
            Assert.IsTrue(testData.Parents.SingleOrDefault(p => p.Name == "Abraham Simpson").Parents.Any(p => p.Name == "Yuma Hickman"));
            Assert.IsFalse(testData.Parents.SingleOrDefault(p => p.Name == "Penelope Olsen").Parents.Any());
        }

        /// <summary>
        /// Intention: Check that Homer Simpson has a partner.
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task HasPartnerAsyncOkTest()
        {
            //ARRANGE
            var testObject = new PeopleRepositoryMongoDb(ConnectionstringTest, DatabaseTest, CollectionTest);

            //ACT
            var testData = await testObject.HasPartnerAsync("Homer");

            //ASSERT
            Assert.IsTrue(testData);
        }

        /// <summary>
        /// Intention: Check that Lisa Simpson has no partner.
        /// Expected: Fail
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task HasPartnerAsyncFailTest()
        {
            //ARRANGE
            var testObject = new PeopleRepositoryMongoDb(ConnectionstringTest, DatabaseTest, CollectionTest);

            //ACT
            var testData = await testObject.HasPartnerAsync("Lisa");

            //ASSERT
            Assert.IsFalse(testData);
        }

        /// <summary>
        /// Intention: Create a new child with name "Test" to Homer Simpson, we check that has a partner.
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AddChildAsyncWithPartnerOkTest()
        {
            //ARRANGE
            var testObject = new PeopleRepositoryMongoDb(ConnectionstringTest, DatabaseTest, CollectionTest);
            var testPerson = GetTestPerson();

            //ACT
            var testData = await testObject.AddChildAsync("Homer", testPerson);

            //ASSERT
            Assert.IsNotNull(testData);
            Assert.IsFalse(Guid.Parse(testData) != Guid.Empty);
        }

        /// <summary>
        /// Intention: Create a new child with name "Test" to Lisa SImpson, we check that has no correct partner.
        /// Expected: Failure
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AddChildAsyncWithNoPartnerFailTest()
        {
            //ARRANGE
            var testObject = new PeopleRepositoryMongoDb(ConnectionstringTest, DatabaseTest, CollectionTest);
            var testPerson = GetTestPerson();

            //ACT
            var testData = await testObject.AddChildAsync("Lisa", testPerson);

            //ASSERT
            Assert.IsNull(testData);
        }


        // Private Methods
        private static void GetTestConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            ConnectionstringTest = configuration.GetConnectionString("MongoDbConnectionTest");
            DatabaseTest = configuration.GetSection("MongoDbConfig").GetSection("Database").Value;
            CollectionTest = configuration.GetSection("MongoDbConfig").GetSection("Collection").Value;
        }

        private static void CleanDb()
        {
            var client = new MongoClient(ConnectionstringTest);
            var database = client.GetDatabase(DatabaseTest);

            database.DropCollection(CollectionTest);
        }

        private static void SeedDb()
        {
            var file = File.ReadAllLines("TestData\\GenoomSimpsons.Data.json");

            var client = new MongoClient(ConnectionstringTest);
            var database = client.GetDatabase(DatabaseTest);
            database.CreateCollection(CollectionTest);
            var collection = database.GetCollection<Person>(CollectionTest);

            collection.InsertOne(GetTestPerson());
        }

        private static Person GetTestPerson()
        {
            return new Person {
                Name = "TestName",
                LastName = "TestLastName",
                Sex = SexEnum.Male,
                PhotoFileName = "TestPhoto.jpg",
                BirthDate = new DateTime(1992, 07, 24)
            };
        }
    }
}
