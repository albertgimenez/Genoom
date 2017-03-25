using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
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
        public async Task<Person> GetPersonAsync(Guid id)
        {
            return await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id, DbType.Guid);

                return await connection.QueryFirstOrDefaultAsync<Person>(
                    sql: "SELECT * FROM Person WHERE Id = @Id",
                    param: parameters,
                    commandType: CommandType.Text);
            });
        }

        public async Task<IEnumerable<PersonFamily>> GetFamilyAsync(Guid id)
        {
            return await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id, DbType.Guid);

                return await connection.QueryAsync<PersonFamily>(
                    sql: "SELECT RelatedPersonId Id, RelatedName Name, RelatedLastName LastName, Birthdate, Sex, PhotoFileName, Relationship " +
                         "FROM PersonRelationshipView " +
                         "WHERE PersonId = @Id",
                    param: parameters,
                    commandType: CommandType.Text);
            });
        }

        public async Task<PersonWithParents> GetTreeAsync(Guid id)
        {
            var topPerson = await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id, DbType.Guid);

                return await connection.QuerySingleOrDefaultAsync<PersonWithParents>(
                    sql: "SELECT RelatedPersonId Id, RelatedName + ' ' + RelatedLastName Name " +
                         "FROM PersonRelationshipView " +
                         "WHERE PersonId = @Id",
                    param: parameters,
                    commandType: CommandType.Text);
            });

            topPerson.Parents = await GetParentsRecursiveAsync(id);
            return topPerson;
        }

        public async Task<bool> HasPartnerAsync(Guid id)
        {
            return await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id, DbType.Guid);
                parameters.Add("Relationship", (short)RelationshipEnum.Partner, DbType.Int16);

                var partnersCount = await connection.ExecuteScalarAsync(
                    sql: "SELECT COUNT(*) FROM PersonFamily WHERE PersonId = @Id AND RelationShip = @Relationship",
                    param: parameters,
                    commandType: CommandType.Text);

                return partnersCount != null;
            });
        }

        public async Task<Guid> AddChildAsync(Guid parentId, Person child)
        {
            return await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ParentId", parentId, DbType.Guid);
                parameters.Add("@Name", child.Name, DbType.String);
                parameters.Add("@LastName", child.LastName, DbType.String);
                parameters.Add("@Birthdate", child.Birthdate, DbType.Date);
                parameters.Add("@Sex", (short)child.Sex, DbType.Int16);
                parameters.Add("@PhotoFileName", child.PhotoFileName, DbType.Int16);

                return await connection.ExecuteScalarAsync<Guid>(
                    sql: "AddChild",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        // Private Methods
        private async Task<IEnumerable<PersonWithParents>> GetParentsRecursiveAsync(Guid id)
        {
            var parents = await GetParentsSql(id);

            // Base case
            if (parents == null)
            {
                return null;
            }

            // Inductive Case
            var parentsList = new List<PersonWithParents>();
            foreach (var parent in parents)
            {
                var parentsOfparent = await GetParentsRecursiveAsync(parent.Id);
                if (parentsOfparent != null) { parentsList.AddRange(parentsOfparent); }
            }

            return parentsList;
        }

        private async Task<IEnumerable<PersonWithParents>> GetParentsSql(Guid id)
        {
            var parents = await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id, DbType.Guid);
                parameters.Add("RelationShip", (short) RelationshipEnum.Parent, DbType.Int16);

                return await connection.QueryAsync<PersonWithParents>(
                    sql: "SELECT RelatedPersonId Id, RelatedName + ' ' + RelatedLastName Name " +
                         "FROM PersonRelationshipView " +
                         "WHERE PersonId = @Id AND Relationship = @RelationShip",
                    param: parameters,
                    commandType: CommandType.Text);
            });

            return parents;
        }
    }
}