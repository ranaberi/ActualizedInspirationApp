using System.ComponentModel.DataAnnotations;

namespace SuggestionAppUI.Models;
/// <summary>
/// CreateSuggestionModel which has UI specific attributes is created here instead of the class Library
/// </summary>
public class CreateSuggestionModel
{
    [Required]
    [MaxLength(75)]
    public string Suggestion { get; set; }
    [Required]
    [MinLength(1)]
    [Display(Name = "Category")]
    public string CategoryId { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }
}
