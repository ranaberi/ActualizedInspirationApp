using System.ComponentModel.DataAnnotations;

namespace SuggestionAppUI.Models;
/// <summary>
/// CreateSuggestionModel which has UI specific attributes is created here instead of the class Library.
/// CreateSuggestionModel is different than the suggestion model because it imposes some limitations on what you can create.
/// CreateSuggestionModel is created just for the form of create suggestion page
/// </summary>
public class CreateSuggestionModel
{
    [Required]
    [MaxLength(75)]
    public string Suggestion { get; set; }

    /// <summary>
    /// MinLength(1) to avoid the empty string case.
    /// Display(Name = "Category") ton ensure that when an error occurs the word Category is used inseated of CategoryId
    /// </summary>
    [Required]
    [MinLength(1)]
    [Display(Name = "Category")]
    public string CategoryId { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }
}
