using System.Collections.Generic;

namespace Genoom.Simpsons.Model
{
    public class PersonFamily : Person
    {
        public ICollection<PersonRelationship> FamilyMembers { get; set; }
    }
}
