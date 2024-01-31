using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace SuggestionAppLibrary.DataAccess;

public class DBConnection : IDBConnection
{
    private readonly IConfiguration _configuration;

    //database is just usuable internally (private)
    private readonly IMongoDatabase _db;

    //_connectionId references the connection string "MongoDB" in appsettings.json
    private string _connectionId = "MongoDB";
    public string DbName { get; private set; }

    //Collections that are exposed externally (public)
    public string CategoryCollectionName { get; private set; } = "categories";
    public string StatusCollectioName { get; private set; } = "statuses";
    public string UserCollectionName { get; private set; } = "users";
    public string SuggestionCollectionName { get; private set; } = "suggestions";

    //This client connects to the Database (DbName) then connects to the collections below (tables) in order to get the needed data.
    //It is exposed externally because we need to connect to the collections outside this db connection (inside the classes that talk to mongodb)
    public MongoClient Client { get; private set; }
    public IMongoCollection<CategoryModel> CategoryCollection { get; private set; }
    public IMongoCollection<StatusModel> StatusCollection { get; private set; }
    public IMongoCollection<UserModel> UserCollection { get; private set; }
    public IMongoCollection<SuggestionModel> SuggestionCollection { get; private set; }

    /// <summary>
    /// Constructor that creates a new client, a connection to the database and then a connection to all four collections.
    /// When put into a dependency injection, It will make this as a singleton then everytime called the same instance is used rather than re-instantiating 
    /// the db collection. As a result the connection to the databse and collection is done only once then it is reused for better performance 
    /// </summary>
    /// <param name="configuration"></param>
    public DBConnection(IConfiguration configuration)
    {
        _configuration = configuration;
        Client = new MongoClient(_configuration.GetConnectionString(_connectionId));
        //returns the string value of the database name from DatabaseName in the root of appsettings.json
        DbName = _configuration["DatabaseName"];
        _db = Client.GetDatabase(DbName);

        CategoryCollection = _db.GetCollection<CategoryModel>(CategoryCollectionName);
        StatusCollection = _db.GetCollection<StatusModel>(StatusCollectioName);
        UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
        SuggestionCollection = _db.GetCollection<SuggestionModel>(SuggestionCollectionName);
    }
}
