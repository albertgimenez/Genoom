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
        public IEnumerable<Tuple<Person, PersonRelationshipEnum>> FamilyMembers { get; set; }
    }
}
