namespace Genoom.Simpsons.Model
{
    public enum RelationshipEnum
    {
        Partner,
        Parent,
        Sibling,
        Child
    }

    public class PersonRelationship
    {
        public Person Person { get; set; }
        public RelationshipEnum Relationship { get; set; }
    }
}
