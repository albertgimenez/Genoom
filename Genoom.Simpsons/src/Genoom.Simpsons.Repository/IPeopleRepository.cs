using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Genoom.Simpsons.Model;

namespace Genoom.Simpsons.Repository
{
    public interface IPeopleRepository
    {
        // Read
        Task<Person> GetPersonAsync(Guid id);
        Task<IEnumerable<PersonFamily>> GetFamilyAsync(Guid id);
        Task<PersonWithParents> GetTreeAsync(Guid id);
        Task<bool> HasPartnerAsync(Guid id);

        // Update
        Task<Guid> AddChildAsync(Guid parentId, Person child);
    }
}
