using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Genoom.Simpsons.Model;

namespace Genoom.Simpsons.Repository.MongoDb
{
    public class PeopleRepositoryMongoDb : IPeopleRepository
    {
        #region Implementation of IPeopleRepository

        public async Task<Person> GetPersonAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PersonRelationship>> GetFamilyAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Person>> GetTreeAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> HasPartnerAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> AddChildAsync(Person child, Guid personId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
