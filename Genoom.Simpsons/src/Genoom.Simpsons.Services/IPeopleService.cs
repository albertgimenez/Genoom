using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Genoom.Simpsons.Model;

namespace Genoom.Simpsons.Services
{
    public interface IPeopleService
    {
        // Read
        Task<Person> GetAsync(Guid id);
        Task<IEnumerable<Person>> GetFamilyAsync(Guid id);
        Task<IEnumerable<Person>> GetChildrenAsync(Guid id);
        Task<IEnumerable<Person>> GetTreeAsync(Guid id);

        // Update
        Task<Guid> AddChildrenAsync(Person child, Guid personId);
    }
}
