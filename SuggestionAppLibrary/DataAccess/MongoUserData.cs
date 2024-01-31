

using System.Data;

namespace SuggestionAppLibrary.DataAccess;

public class MongoUserData : IUserData
{
    //a copy of the reference to the object (not the actual object)
    private readonly IMongoCollection<UserModel> _users;
    public MongoUserData(IDBConnection db)
    {
        _users = db.UserCollection;
    }

    /// <summary>
    /// Getting all the users in the list asynchronously
    /// </summary>
    /// <returns></returns>
    public async Task<List<UserModel>> GetUsersAsync()
    {
        //It finds all the records where true is true so essentialy all records
        var results = await _users.FindAsync(_ => true);
        return results.ToList();
    }
    /// <summary>
    /// Returns the user that his id matches the id passed in
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserModel> GetUser(string id)
    {
        var results = await _users.FindAsync(u => u.Id == id);
        return results.FirstOrDefault();
    }

    /// <summary>
    /// Returns the user that his ObjectIdentifier (given by the azure B2C) matches the objectId passed in
    /// </summary>
    /// <param name="objectId"></param>
    /// <returns></returns>
    public async Task<UserModel> GetUserFromAuthentication(string objectId)
    {
        var results = await _users.FindAsync(u => u.ObjectIdentifier == objectId);
        return results.FirstOrDefault();
    }
    /// <summary>
    /// Creating a new user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>

    public Task CreateUser(UserModel user)
    {
        return _users.InsertOneAsync(user);
    }
    /// <summary>
    /// Creates a filter for the replace that matches the Id to the user.Id
    /// ReplaceOneAsync finds the filter object and puts the user in its place (with matching Ids already)
    /// IsUpsert ( noEntryFound? insert : update )
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public Task UpdateUser(UserModel user)
    {
        var filter = Builders<UserModel>.Filter.Eq("Id", user.Id);
        return _users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
    }

}
