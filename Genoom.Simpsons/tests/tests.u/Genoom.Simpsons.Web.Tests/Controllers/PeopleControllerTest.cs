using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Genoom.Simpsons.Model;
using Genoom.Simpsons.Repository;
using Genoom.Simpsons.Web.Controllers;

namespace Genoom.Simpsons.Web.Tests.Controllers
{
    [TestClass]
    public class PeopleControllerTest
    {
        // Public Methods
        /// <summary>
        /// Intention: Get a test person data.
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetPersonAsyncOkTest()
        {
            //ARRANGE
            var mockedRepository = new Mock<IPeopleRepository>();
            mockedRepository
                .Setup(t => t.GetPersonAsync(It.IsAny<string>()))
                .Returns(GetTestPersonData());
            var testObject = new PeopleController(mockedRepository.Object);

            //ACT
            var testData = await testObject.Get("TestName");

            //ASSERT
            Assert.IsNotNull(testData);
        }

        /// <summary>
        /// Intention: Get the family tree for "TestName", and check he has the correct family members in all the tree directions.
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetFamilyAsyncOkTest()
        {
            //ARRANGE
            var mockedRepository = new Mock<IPeopleRepository>();
            mockedRepository
                .Setup(t => t.GetFamilyAsync(It.IsAny<string>()))
                .Returns(GetTestFamilyData());
            var testObject = new PeopleController(mockedRepository.Object);

            //ACT
            var testData = await testObject.GetFamily("TestName");

            //ASSERT
            Assert.IsNotNull(testData);
        }

        /// <summary>
        /// Intention: Simulate creating a new child to "TestName", we assume it has correct partner.
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task PostAddChildAsyncWithPartnerOkTest()
        {
            //ARRANGE
            var mockedRepository = new Mock<IPeopleRepository>();
            mockedRepository
                .Setup(t => t.HasPartnerAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            mockedRepository
                .Setup(t => t.AddChildAsync(It.IsAny<string>(), It.IsAny<Person>()))
                .Returns(Task.FromResult(Guid.NewGuid().ToString()));
            var testObject = new PeopleController(mockedRepository.Object);

            //ACT
            var testData = await testObject.GetFamily("TestName");

            //ASSERT
            Assert.IsNotNull(testData);
        }

        /// <summary>
        /// Intention: Simulate creating a new child to "TestName", we assume it has NO partner.
        /// Expected: Failure
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task PostAddChildAsyncWithNoPartnerFailTest()
        {
            //ARRANGE
            var mockedRepository = new Mock<IPeopleRepository>();
            mockedRepository
                .Setup(t => t.HasPartnerAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            mockedRepository
                .Setup(t => t.AddChildAsync(It.IsAny<string>(), It.IsAny<Person>()))
                .Returns(Task.FromResult(Guid.NewGuid().ToString()));
            var testObject = new PeopleController(mockedRepository.Object);

            //ACT
            var testData = await testObject.GetFamily("TestName");

            //ASSERT
            Assert.IsNotNull(testData);
        }


        // Private Methods
        private static Task<Person> GetTestPersonData()
        {
            return Task.FromResult(new Person {
                Id = Guid.NewGuid(),
                Name = "TestName",
                LastName = "TestLastName",
                BirthDate = new DateTime(1980, 03, 01),
                PhotoFileName = "TestPhoto.jpg",
                Sex = SexEnum.Male
            });
        }

        private static Task<IEnumerable<PersonFamily>> GetTestFamilyData()
        {
            return Task.FromResult(new List<PersonFamily>
            {
                new PersonFamily
                {
                    Id = Guid.NewGuid(),
                    Name = "TestParent1",
                    Relationship = RelationshipEnum.Parent
                },
                new PersonFamily
                {
                    Id = Guid.NewGuid(),
                    Name = "TestSibling1",
                    Relationship = RelationshipEnum.Sibling
                },
                new PersonFamily
                {
                    Id = Guid.NewGuid(),
                    Name = "TestPartner1",
                    Relationship = RelationshipEnum.Partner
                },
                new PersonFamily
                {
                    Id = Guid.NewGuid(),
                    Name = "TestChild1",
                    Relationship = RelationshipEnum.Child
                }
            }.AsEnumerable());
        }
    }
}
