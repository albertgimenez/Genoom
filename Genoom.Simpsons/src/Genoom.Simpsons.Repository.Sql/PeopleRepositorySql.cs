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
        public PeopleRepositorySql(string connectionstring) : base(connectionstring)
        {
            ConnectionString = connectionstring;
        }

        // Methods
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

        public async Task<IEnumerable<PersonRelationship>> GetFamilyAsync(Guid id)
        {
            return await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id, DbType.Guid);

                return await connection.QueryAsync<PersonRelationship>(
                    sql: "SELECT RelatedPersonId Id, RelatedName Name, RelatedLastName Lastname, Birthdate, Sex, PhotoFileName, Relationship " +
                         "FROM PersonRelationshipView " +
                         "WHERE PersonId = @Id",
                    param: parameters,
                    commandType: CommandType.Text);
            });
        }

        public async Task<IEnumerable<Person>> GetTreeAsync(Guid id)
        {
            return await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id, DbType.Guid);

                return await connection.QueryAsync<PersonRelationship>(
                    sql: "SELECT RelatedPersonId Id, RelatedName Name, RelatedLastName Lastname, Birthdate, Sex, PhotoFileName " +
                         "FROM PersonRelationshipView " +
                         "WHERE PersonId = @Id AND Relationship = 1",
                    param: parameters,
                    commandType: CommandType.Text);
            });
        }

        public async Task<bool> HasPartnerAsync(Guid id)
        {
            return await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", id, DbType.Guid);
                parameters.Add("Relationship", (short)RelationshipEnum.Parent, DbType.Int16);

                var partnersCount = await connection.ExecuteScalarAsync(
                    sql: "SELECT COUNT(*) FROM PersonFamily WHERE PersonId = @Id AND RelationShip = @Relationship",
                    param: parameters,
                    commandType: CommandType.Text);

                return partnersCount != null;
            });
        }

        public async Task<Guid> AddChildAsync(Person child, Guid parentId)
        {
            return await WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ParentId", parentId, DbType.Guid);
                parameters.Add("@Name", child.Name, DbType.String);
                parameters.Add("@Lastname", child.Lastname, DbType.String);
                parameters.Add("@Birthdate", child.Birthdate, DbType.Date);
                parameters.Add("@Sex", (short)child.Sex, DbType.Int16);
                parameters.Add("@PhotoFileName", child.PhotoFileName, DbType.Int16);

                return await connection.ExecuteScalarAsync<Guid>(
                    sql: "AddChild",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}