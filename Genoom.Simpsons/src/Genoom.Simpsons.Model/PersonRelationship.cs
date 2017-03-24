namespace Genoom.Simpsons.Model
{
    public enum RelationshipEnum
    {
        Partner,
        Parent,
        Sibling,
        Child
    }

    public class PersonRelationship : Person
    {
        public RelationshipEnum Relationship { get; set; }
    }
}
