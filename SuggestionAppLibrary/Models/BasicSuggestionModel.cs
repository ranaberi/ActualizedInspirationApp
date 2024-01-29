namespace SuggestionAppLibrary.Models;

public class BasicSuggestionModel
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Suggestion { get; set; }
    public BasicSuggestionModel()
    {
        
    }
    /// <summary>
    /// A constructor that takes the full object and convert it to this basic object
    /// </summary>
    /// <param name="suggestion"></param>
    public BasicSuggestionModel(SuggestionModel suggestion)
    {
        Id = suggestion.Id;
        Suggestion = suggestion.Suggestion;
        
    }
}
