using Microsoft.Extensions.Caching.Memory;

namespace SuggestionAppLibrary.DataAccess;

public class MongoCategoryData : ICategoryData
{
    private readonly IMongoCollection<CategoryModel> _categories;
    private readonly IMemoryCache _cache;
    private const string CacheName = "CategoryData";

    //The category data won't change that often, so we cache it for a long time
    public MongoCategoryData(IDBConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _categories = db.CategoryCollection;
    }
    /// <summary>
    /// Retrieves from the cache by key CacheName a list of CategoryModel
    /// Since the first time we don't have a cache yet, we retrieve the data from the database then we put it in the cache for one day.
    /// </summary>
    /// <returns></returns>
    public async Task<List<CategoryModel>> GetAllCategories()
    {
        var output = _cache.Get<List<CategoryModel>>(CacheName);
        if (output is null)
        {
            var results = await _categories.FindAsync(_ => true);
            output = results.ToList();
            _cache.Set(CacheName, output, TimeSpan.FromDays(1));
        }
        return output;

    }
    /// <summary>
    /// Create a category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public Task CreateCategory(CategoryModel category)
    {
        return _categories.InsertOneAsync(category);
    }
}
