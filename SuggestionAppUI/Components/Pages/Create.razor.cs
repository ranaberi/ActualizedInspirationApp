using SuggestionAppUI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace SuggestionAppUI.Components.Pages
{
    public partial class Create
    {
        private CreateSuggestionModel suggestion = new();
        private List<CategoryModel> categories;
        private UserModel loggedInUser;

        protected async override Task OnInitializedAsync()
        {
            categories = await categoryData.GetAllCategories();

            var authState = await authProvider.GetAuthenticationStateAsync();
            string objectId = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;
            loggedInUser = await userData.GetUserFromAuthentication(objectId);

        }
        private void ClosePage()
        {
            navManager.NavigateTo("/");
        }

        /// <summary>
        /// Translating the CreateSuggestionModel to a SuggestionModel by manually mapping it.
        /// </summary>
        /// <returns></returns>
        private async Task CreateSuggestion()
        {
            SuggestionModel s = new();
            s.Suggestion = suggestion.Suggestion;
            s.Description = suggestion.Description;
            s.Author = new BasicUserModel(loggedInUser);

            //Finds the category matching the selected option.
            s.Category = categories.Where(c => c.Id == suggestion.CategoryId).FirstOrDefault();

            //If no entry is found
            if (s.Category is null)
            {
                suggestion.CategoryId = "";
                return;
            }
            await suggestionData.CreateSuggestion(s);
            //Re-institiating the suggestion just in case
            suggestion = new();
            ClosePage();
        }
    }
}