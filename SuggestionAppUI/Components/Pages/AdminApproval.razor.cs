using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuggestionAppUI.Components.Pages
{
    public partial class AdminApproval
    {
        private List<SuggestionModel> submissions;
        private SuggestionModel editingModel;
        private string currentEditingTitle = "";
        private string editedTitle = "";
        private string currentEditingDescription = "";
        private string editedDescription;

        /// <summary>
        /// Loads pending suggestions
        /// </summary>
        /// <returns></returns>
        protected async override Task OnInitializedAsync()
        {
            submissions = await suggestionData.GetAllSuggestionsWaitingForApproval();
        }
        /// <summary>
        /// After approving the suggestion, it gets removed from the submissions list (pending suggestions)
        /// </summary>
        /// <param name="submission"></param>
        /// <returns></returns>
        private async Task ApproveSubmission(SuggestionModel submission)
        {
            submission.ApprovedForRelease = true;
            submissions.Remove(submission);
            await suggestionData.UpdateSuggestion(submission);
        }

        private async Task RejectSubmission(SuggestionModel submission)
        {
            submission.Rejected = true;
            submissions.Remove(submission);
            await suggestionData.UpdateSuggestion(submission);
        }
        /// <summary>
        /// Enables the editing of the model in text area after being clicked. 
        /// assignes the new suggestion model (editingModel) to the model in the text area
        /// </summary>
        /// <param name="model"></param>
        private void EditTitle(SuggestionModel model)
        {
            editingModel = model;
            editedTitle = model.Suggestion;
            currentEditingTitle = model.Id;
            currentEditingDescription = "";
        }
        /// <summary>
        /// Close out the field that was being edited. Update the model with the new title and send it to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task SaveTitle(SuggestionModel model)
        {
            //In order to close the title section in the form
            currentEditingTitle = string.Empty;
            model.Suggestion = editedTitle;
            await suggestionData.UpdateSuggestion(model);
        }

        private void EditDescription(SuggestionModel model)
        {
            editingModel = model;
            editedDescription = model.Description;
            currentEditingTitle = "";
            currentEditingDescription = model.Id;
        }

        private async Task SaveDescription(SuggestionModel model)
        {
            currentEditingDescription = string.Empty;
            model.Description = editedDescription;
            await suggestionData.UpdateSuggestion(model);
        }
        private void ClosePage()
        {
            navManager.NavigateTo("/");
        }

    }
}