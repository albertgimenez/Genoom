using System;
using System.Collections.Generic;

namespace Genoom.Simpsons.Model
{
    public enum PersonRelationshipEnum
    {
        Partner,
        Parent,
        Sibling,
        Child
    }

    public class PersonFamily : Person
    {
        public ICollection<Tuple<Person, PersonRelationshipEnum>> FamilyMembers { get; set; }
    }
}
