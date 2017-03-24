using System;
using System.Collections.Generic;

namespace Genoom.Simpsons.Model
{
    public class PersonWithParents
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PersonWithParents> Parents { get; set; }
    }
}
