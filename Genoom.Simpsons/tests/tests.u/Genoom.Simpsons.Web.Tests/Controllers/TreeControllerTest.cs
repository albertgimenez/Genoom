using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Genoom.Simpsons.Model;
using Genoom.Simpsons.Repository;
using Genoom.Simpsons.Web.Controllers;

namespace Genoom.Simpsons.Web.Tests.Controllers
{
    [TestClass]
    public class TreeControllerTest
    {
        // Public Methods
        /// <summary>
        /// Intention: Call the Get action for the test tree. We check that from the root we can navigate up to the parents and grandparents.
        /// Expected: Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetOkTest()
        {
            //ARRANGE
            var mockedRepository = new Mock<IPeopleRepository>();
            mockedRepository
                .Setup(t => t.GetTreeAsync(It.IsAny<string>()))
                .Returns(GetTestData());
            var testObject = new TreeController(mockedRepository.Object);

            //ACT
            var testData = await testObject.Get("TestName");

            //ASSERT
            Assert.IsNotNull(testData);
            //Assert.IsTrue(testData.Parents.Any(p => p.Name == "TestParent1"));
            //Assert.IsTrue(testData.Parents.Any(p => p.Name == "TestParent2"));
            //Assert.IsTrue(testData.Parents.SingleOrDefault(p => p.Name == "TestParent1").Parents.Any(p => p.Name == "TestGrandParent1"));
            //Assert.IsTrue(testData.Parents.SingleOrDefault(p => p.Name == "TestParent2").Parents.Any(p => p.Name == "TestGrandParent2"));
            //Assert.IsTrue(testData.Parents.SingleOrDefault(p => p.Name == "TestParent2").Parents.Any(p => p.Name == "TestGrandParent3"));
            //Assert.IsFalse(testData.Parents.SingleOrDefault(p => p.Name == "TestParent1").Parents.Any());
        }

        // Private Methods
        private static Task<PersonWithParents> GetTestData()
        {
            return Task.FromResult(new PersonWithParents {
                Id = Guid.NewGuid(),
                Name = "TestName",
                Parents = new List<PersonWithParents>
                {
                    new PersonWithParents
                    {
                        Id = Guid.NewGuid(),
                        Name = "TestParent1",
                        Parents = new List<PersonWithParents>
                        {
                            new PersonWithParents
                            {
                                Id = Guid.NewGuid(),
                                Name = "TestGrandParent1",
                                Parents = null
                            }
                        }
                    },
                    new PersonWithParents
                    {
                        Id = Guid.NewGuid(),
                        Name = "TestParent2",
                        Parents = new List<PersonWithParents>
                        {
                            new PersonWithParents
                            {
                                Id = Guid.NewGuid(),
                                Name = "TestGrandParent2",
                                Parents = null
                            },
                            new PersonWithParents
                            {
                                Id = Guid.NewGuid(),
                                Name = "TestGrandParent3",
                                Parents = null
                            }
                        }
                    }
                }
            });
        }
    }
}
