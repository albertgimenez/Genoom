using System;
using System.ComponentModel.DataAnnotations;

namespace Genoom.Simpsons.Model
{
    public enum PersonSexEnum
    {
        Male,
        Female
    }

    public class Person
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; }
        public PersonSexEnum Sex { get; set; }
        public string PhotoFileName { get; set; }
    }
}
