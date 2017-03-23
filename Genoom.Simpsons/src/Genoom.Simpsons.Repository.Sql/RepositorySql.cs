//using Dapper;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using static System.Data.CommandType;
//using Genoom.Simpsons.Model;
//using Genoom.Simpsons.Repository;

//namespace Genoom.Simpsons.Repository.Sql
//{
//    public class UserRepositoryBaseRepository //: IUserRepository
//    {
//    public IList<Person> GetPerson()
//    {
//        IList<User> customerList = SqlMapper.Query<User>(con, "GetAllUsers", commandType StoredProcedure).ToList();
//        return customerList;
//    }
//    publicUser GetUserById(int userId)
//    {
//        try {
//            DynamicParameters parameters = newDynamicParameters();
//            parameters.Add("@CustomerID", userId);
//            returnSqlMapper.Query<User>((SqlConnection)con, "GetUserById", parameters, commandType StoredProcedure).FirstOrDefault();
//        } catch (Exception) {
//            throw;
//        }
//    }
//    publicbool AddChildToPerson(Guid idParent, Person child)
//    {
//        try {
//            DynamicParameters parameters = new DynamicParameters();
//            parameters.Add("@UserId", user.UserId);
//            parameters.Add("@UserName", user.UserName);
//            parameters.Add("@UserMobile", user.UserMobile);
//            parameters.Add("@UserEmail", user.UserEmail);
//            parameters.Add("@FaceBookUrl", user.FaceBookUrl);
//            parameters.Add("@LinkedInUrl", user.LinkedInUrl);
//            parameters.Add("@TwitterUrl", user.TwitterUrl);
//            parameters.Add("@PersonalWebUrl", user.PersonalWebUrl);
//            SqlMapper.Execute(con, "UpdateUser", param parameters, commandType StoredProcedure);
//            return true;
//        } catch (Exception ex) {
//            throw ex;
//        }
//    }
//}
//}