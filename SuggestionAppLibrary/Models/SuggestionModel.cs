namespace SuggestionAppLibrary.Models;

public class SuggestionModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Suggestion { get; set; }
    public string Description { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public CategoryModel Category { get; set; }
    public BasicUserModel Author { get; set; }

    public HashSet<string> UserVotes { get; set; } = new();
    public StatusModel SuggestionStatus { get; set; }
    public string OwnerNotes { get; set; }
    
    //false by default until we approve it
    public bool ApprovedForRelease { get; set; } = false;
    //Not archived by default
    public bool Archived { get; set; } = false;
    //Once rejected it won't be shown anywhere except the authors's list
    public bool Rejected { get; set; } = false;
}
