using Microsoft.AspNetCore.Components.Forms;
namespace SuggestionAppUI.Components;

/// <summary>
/// Since the EditForm re-renders the page for every click event, the standard InputRadioGroup does not register the first click.
/// This CustomInputRadioGroup solves this issue.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class CustomInputRadioGroup<TValue>: InputRadioGroup<TValue>
{
    private string _name;
    private string _fieldClass;

    /// <summary>
    /// Get the field identifier or puts an empty string if none exists
    /// </summary>
    protected override void OnParametersSet()
    {
        var fieldClass = EditContext?.FieldCssClass(FieldIdentifier) ?? string.Empty;
        if(fieldClass != _fieldClass || Name != _name)
        {
            _fieldClass = fieldClass;
            _name = Name;
        }
        base.OnParametersSet();
    }
}
