
using MongoDB.Driver;
using System.Data;

namespace SuggestionAppLibrary.DataAccess;

public class MongoUserData
{
    private readonly IMongoCollection<UserModel> _users;
    public MongoUserData(IDbConnection db)
    {
        
    }
}
