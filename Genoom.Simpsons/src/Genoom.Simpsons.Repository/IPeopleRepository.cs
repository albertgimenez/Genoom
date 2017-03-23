using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Genoom.Simpsons.Model;

namespace Genoom.Simpsons.Repository
{
    public interface IPeopleRepository
    {
        // Read
        Task<Person> ReadAsync(Guid id);
        Task<IEnumerable<Person>> ReadFamilyAsync(Guid id);
        Task<IEnumerable<Person>> ReadChildrenAsync(Guid id);
        Task<IEnumerable<Person>> ReadTreeAsync(Guid id);
        Task<bool> HasPartnerAsync(Guid id);

        // Update
        Task<Guid> AddChildrenAsync(Person child, Guid personId);
    }
}
