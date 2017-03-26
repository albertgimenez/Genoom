using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Genoom.Simpsons.Model;

namespace Genoom.Simpsons.Repository
{
    public interface IPeopleRepository
    {
        // Read
        Task<Person> GetPersonAsync(string id);
        Task<IEnumerable<PersonFamily>> GetFamilyAsync(string id);
        Task<PersonWithParents> GetTreeAsync(string id);
        Task<bool> HasPartnerAsync(string id);

        // Update
        Task<string> AddChildAsync(string parentId, Person child);
    }
}
