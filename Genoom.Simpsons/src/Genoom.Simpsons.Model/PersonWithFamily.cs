using System.Collections.Generic;

namespace Genoom.Simpsons.Model
{
    public class PersonWithFamily : Person
    {
        public ICollection<Person> Partners { get; set; }
        public ICollection<Person> Parents { get; set; }
        public ICollection<Person> Siblings { get; set; }
        public ICollection<Person> Children { get; set; }
    }
}
