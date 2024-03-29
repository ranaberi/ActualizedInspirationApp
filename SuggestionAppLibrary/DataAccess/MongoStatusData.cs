﻿using Microsoft.Extensions.Caching.Memory;

namespace SuggestionAppLibrary.DataAccess;

public class MongoStatusData : IStatusData
{
    private readonly IMongoCollection<StatusModel> _statuses;
    private readonly IMemoryCache _cache;
    private const string CacheName = "StatusData";

    public MongoStatusData(IDBConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _statuses = db.StatusCollection;
    }

    /// <summary>
    /// Retreives the statuses from the database if cache is empty and cache it for one day.
    /// Else it retrieves the statuses from the cache.
    /// </summary>
    /// <returns></returns>
    public async Task<List<StatusModel>> GetAllStatuses()
    {
        var output = _cache.Get<List<StatusModel>>(CacheName);
        if (output is null)
        {
            var results = await _statuses.FindAsync(_ => true);
            output = results.ToList();

            _cache.Set(CacheName, output, TimeSpan.FromDays(1));
        }
        return output;
    }
    /// <summary>
    /// Creates a status
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task CreateStatus(StatusModel status)
    {
        return _statuses.InsertOneAsync(status);
    }
}
