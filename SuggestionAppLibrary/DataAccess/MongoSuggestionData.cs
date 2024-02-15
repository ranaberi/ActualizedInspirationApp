

using Microsoft.Extensions.Caching.Memory;
using System.Runtime.InteropServices;

namespace SuggestionAppLibrary.DataAccess;

public class MongoSuggestionData : ISuggestionData
{
    private readonly IDBConnection _db;
    private readonly IUserData _userData;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<SuggestionModel> _suggestions;
    private const string CacheName = "SuggestionData";

    public MongoSuggestionData(IDBConnection db, IUserData userData, IMemoryCache cache)
    {
        _db = db;
        _userData = userData;
        _cache = cache;
        _suggestions = db.SuggestionCollection;
    }
    /// <summary>
    /// Retrieves the suggestions (except the archived ones) from the databse then cache it for one minute for the first time
    /// If cache found it retrieves the suggestions from it
    /// </summary>
    /// <returns></returns>
    public async Task<List<SuggestionModel>> GetAllSuggestions()
    {
        var output = _cache.Get<List<SuggestionModel>>(CacheName);
        if (output is null)
        {
            var results = await _suggestions.FindAsync(s => s.Archived == false);
            output = results.ToList();
            _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
        }
        return output;
    }

    /// <summary>
    /// Retrieves the suggestions based on the author Id
    /// cached for a minute in case the users comes back to the profile page multiple times.
    /// better than reloadig the usggestions over and over
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<SuggestionModel>> GetUserSuggestions(string userId)
    {
        var output = _cache.Get<List<SuggestionModel>>(userId);
        if (output is null)
        {
            var results = await _suggestions.FindAsync(s => s.Author.Id == userId);
            output = results.ToList();

            _cache.Set(userId, output, TimeSpan.FromMinutes(1));
        }
        return output;
    }

    /// <summary>
    /// Returns only the suggestions approved for release
    /// </summary>
    /// <returns></returns>
    public async Task<List<SuggestionModel>> GetAllApprovedSuggestions()
    {
        var output = await GetAllSuggestions();
        return output.Where(x => x.ApprovedForRelease).ToList();
    }

    /// <summary>
    /// Returns one suggestion based on the matching id
    /// For one suggestion no need to cache
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SuggestionModel> GetSuggestion(string id)
    {
        var results = await _suggestions.FindAsync(s => s.Id == id);
        return results.FirstOrDefault();
    }


    /// <summary>
    /// Returns all the pending suggestions (not approved and not rejected)
    /// </summary>
    /// <returns></returns>
    public async Task<List<SuggestionModel>> GetAllSuggestionsWaitingForApproval()
    {
        var output = await GetAllSuggestions();
        return output.Where(x => x.ApprovedForRelease == false && x.Rejected == false).ToList();
    }
    /// <summary>
    /// Replace the old suggestion by the new one and removes the old cached suggestion.
    /// </summary>
    /// <param name="suggestion"></param>
    /// <returns></returns>
    public async Task UpdateSuggestion(SuggestionModel suggestion)
    {
        await _suggestions.ReplaceOneAsync(s => s.Id == suggestion.Id, suggestion);
        _cache.Remove(CacheName);
    }

    /// <summary>
    /// Upvoting a suggestion
    /// </summary>
    /// <param name="suggestionId"> the unique identifier of the suggestion to upvote</param>
    /// <param name="userId"> the unique identifier of the user doing the upvoting</param>
    /// <returns></returns>
    public async Task UpvoteSuggestion(string suggestionId, string userId)
    {
        //creates a transaction to make sure when we write to two different collections, it is either succeded or failed and update the user and the suggestion
        var client = _db.Client;
        using var session = await client.StartSessionAsync();

        session.StartTransaction();
        try
        {
            //exposing the database name to connect to it. The databse crerated using this client that is using this session
            var db = client.GetDatabase(_db.DbName);
            var suggestionsInTransaction = db.GetCollection<SuggestionModel>(_db.SuggestionCollectionName);

            //If we don't find the suggestion we are looking for we throw an exception (so we use First() instead of FirstOrDefault())
            var suggestion = (await suggestionsInTransaction.FindAsync(s => s.Id == suggestionId)).First();

            //In the HasheSet of the userVotes which does not allow duplicate entries, Add() tries to add the item
            //itemAdded ? true : false
            bool isUpvote = suggestion.UserVotes.Add(userId);

            // if isUpVote is false means we already upVoted once so we remove the upvote.
            // In the UI, the upvaote arrow once clicked again (after being upvoted) we remove the upvote
            if (isUpvote == false)
            {
                suggestion.UserVotes.Remove(userId);
            }

            //after updating the user votes column, the sugeestion gets updated with the new version just changed
            await suggestionsInTransaction.ReplaceOneAsync(session, s => s.Id == suggestionId, suggestion);
            var userInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
            var user = await _userData.GetUser(userId);

            if (isUpvote)
            {
                user.VotedOnSuggestions.Add(new BasicSuggestionModel(suggestion));
            }
            else
            {
                var suggestionToRemove = user.VotedOnSuggestions.Where(s => s.Id == suggestionId).First();
                user.VotedOnSuggestions.Remove(suggestionToRemove);
            }
            await userInTransaction.ReplaceOneAsync(session, u => u.Id == userId, user);
            await session.CommitTransactionAsync();
            _cache.Remove(CacheName);
        }
        //logging to be added
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }

    /// <summary>
    /// Creating a new suggestion.
    /// Transaction created to enable the user to update his account. It is important to mention that transactions are available only for (non-local) clusters
    /// </summary>
    /// <param name="suggestion"></param>
    /// <returns></returns>
    public async Task CreateSuggestion(SuggestionModel suggestion)
    {
        var client = _db.Client;
        using var session = await client.StartSessionAsync();
        session.StartTransaction();
        try
        {
            var db = client.GetDatabase(_db.DbName);
            var suggestionsInTransaction = db.GetCollection<SuggestionModel>(_db.SuggestionCollectionName);
            await suggestionsInTransaction.InsertOneAsync(session, suggestion);

            var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
            var user = await _userData.GetUser(suggestion.Author.Id);
            user.AuthoredSuggestions.Add(new BasicSuggestionModel(suggestion));
            await usersInTransaction.ReplaceOneAsync(session, u => u.Id == user.Id, user);

            await session.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }
}
