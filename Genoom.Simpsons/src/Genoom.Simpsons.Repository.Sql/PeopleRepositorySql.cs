using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Genoom.Simpsons.Model;

namespace Genoom.Simpsons.Repository.Sql
{
    public class PeopleRepositorySql : BaseRepository, IPeopleRepository
    {
        // Properties
        public string ConnectionString { get; }

        // Ctor
        public PeopleRepositorySql(string connectionString) : base(connectionString)
        {
            ConnectionString = connectionString;
        }

        // Public Methods
        public async Task<Person> GetPersonAsync(string id)
        {
            return await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.String);

                return await connection.QuerySingleOrDefaultAsync<Person>(
                    sql: "SELECT * FROM Person WHERE Name LIKE @Id",
                    param: parameters);
            });
        }

        public async Task<IEnumerable<PersonFamily>> GetFamilyAsync(string id)
        {
            return await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.String);

                return await connection.QueryAsync<PersonFamily>(
                    sql: "SELECT RelatedPersonId Id, RelatedName Name, RelatedLastName LastName, BirthDate, Sex, PhotoFileName, Relationship " +
                         "FROM PersonRelationshipView " +
                         "WHERE Name LIKE @Id",
                    param: parameters);
            });
        }

        public async Task<PersonWithParents> GetTreeAsync(string id)
        {
            var topPerson = await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.String);

                return await connection.QuerySingleOrDefaultAsync<PersonWithParents>(
                    sql: "SELECT Id, Name " +
                         "FROM Person " +
                         "WHERE Name LIKE @Id",
                    param: parameters);
            });

            topPerson.Parents = await GetParentsRecursiveAsync(id);
            return topPerson;
        }

        public async Task<bool> HasPartnerAsync(string id)
        {
            return await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.String);
                parameters.Add("@Relationship", (short)RelationshipEnum.Partner, DbType.Int16);

                var partnersCount = await connection.ExecuteScalarAsync(
                    sql: "SELECT COUNT(*) FROM PersonRelationshipView WHERE Name LIKE @Id AND RelationShip = @Relationship",
                    param: parameters);

                return partnersCount != null;
            });
        }

        public async Task<string> AddChildAsync(string parentId, Person child)
        {
            return await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Parent", parentId, DbType.String);
                parameters.Add("@Name", child.Name, DbType.String);
                parameters.Add("@LastName", child.LastName, DbType.String);
                parameters.Add("@BirthDate", child.BirthDate, DbType.Date);
                parameters.Add("@Sex", (short)child.Sex, DbType.Int16);
                parameters.Add("@PhotoFileName", child.PhotoFileName, DbType.Int16);

                return await connection.ExecuteScalarAsync<string>(
                    sql: "AddChild",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        // Private Methods
        private async Task<IEnumerable<PersonWithParents>> GetParentsRecursiveAsync(string id)
        {
            var parents = await GetParentsSql(id);

            // Base case
            if (!parents.Any())
            {
                return null;
            }

            // Inductive Case
            foreach (var parent in parents)
            {
                var parentsOfparent = await GetParentsRecursiveAsync(parent.Name);
                if (parentsOfparent != null) { parent.Parents = new List<PersonWithParents>(parentsOfparent); }
            }

            return parents;
        }

        private async Task<IEnumerable<PersonWithParents>> GetParentsSql(string id)
        {
            var parents = await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.String);
                parameters.Add("@RelationShip", (short) RelationshipEnum.Parent, DbType.Int16);

                //Note: I use Name = @Id instead of Name LIKE @Id because is more performant and the id will be providen with the exact Name.
                return await connection.QueryAsync<PersonWithParents>(
                    sql: "SELECT RelatedPersonId Id, RelatedName Name " +
                         "FROM PersonRelationshipView " +
                         "WHERE Name = @Id AND Relationship = @RelationShip",
                    param: parameters);
            });

            return parents;
        }
    }
}