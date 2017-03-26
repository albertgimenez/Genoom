using System;
using System.ComponentModel.DataAnnotations;

namespace Genoom.Simpsons.Model
{
    public enum SexEnum
    {
        Male,
        Female
    }

    public class Person
    {
        public Guid Id { get; set; }
        [Key]
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public SexEnum Sex { get; set; }
        public string PhotoFileName { get; set; }
    }
}
