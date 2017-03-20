using System;
using System.ComponentModel.DataAnnotations;

namespace Genoom.Simpsons.Model
{
    public class Person
    {
        [Key]
        private Guid Id { get; set; }
        private string Name { get; set; }
        private string Lastname { get; set; }
        private DateTime Birthdate { get; set; }
    }
}
