using ThirdParty.BouncyCastle.Utilities.IO.Pem;

namespace SuggestionAppLibrary.Models;

public class StatusModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string StartusName { get; set; }
    public string StatusDescription { get; set; }
}
