using SuggestionAppUI.Models;
using System.Collections.Generic;
using System.Linq;
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
            //TODO - Replace with user lookup
            loggedInUser = await userData.GetUserFromAuthentication("1234");
        }
        private void ClosePage()
        {
            navManager.NavigateTo("/");
        }

        /// <summary>
        /// Translating the CreateSuggestionModel to a SuggestionModel
        /// </summary>
        /// <returns></returns>
        private async Task CreateSuggestion()
        {
            SuggestionModel s = new();
            s.Suggestion = suggestion.Suggestion;
            s.Description = suggestion.Description;
            s.Author = new BasicUserModel(loggedInUser);
            s.Category = categories.Where(c => c.Id == suggestion.CategoryId).FirstOrDefault();
            //if no entry is found
            if (s.Category is null)
            {
                suggestion.CategoryId = "";
                return;
            }
            await suggestionData.CreateSuggestion(s);
            //re-institiating the suggestion just in case
            suggestion = new();
            ClosePage();
        }
    }
}