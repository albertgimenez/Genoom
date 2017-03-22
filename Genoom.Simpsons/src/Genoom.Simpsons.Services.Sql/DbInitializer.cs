using System.Linq;

namespace Genoom.Simpsons.Services.Sql
{
    public static class DbInitializer
    {
        public static void Initialize(GenoomSimpsonsContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.People.Any()) {
                return;   // DB has been seeded
            }

            // TODO Add data
            // context.SaveChanges();
        }
    }
}
