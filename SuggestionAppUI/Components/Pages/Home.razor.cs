using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuggestionAppUI.Components.Pages
{
    public partial class Home
    {
        private List<SuggestionModel> suggestions;
        private List<CategoryModel> categories;
        private List<StatusModel> statuses;

        private string selectedCategory = "All";
        private string selectedStatus = "All";
        private string searchText = "";
        bool isSortedByNew = true;


        /// <summary>
        /// Loads categories and statuses data on intilization
        /// </summary>
        /// <returns></returns>
        protected async override Task OnInitializedAsync()
        {
            categories = await CategoryData.GetAllCategories();
            statuses = await StatusData.GetAllStatuses();
        }

        /// <summary>
        /// Runs after the page is rendered
        /// </summary>
        /// <returns></returns>
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadFilterState();
                await FilterSuggestions();
                StateHasChanged();
            }

        }

        /// <summary>
        /// Populates the values used for filtering
        /// </summary>
        /// <returns></returns>
        private async Task LoadFilterState()
        {
            var stringResults = await ProtectedSessionStorage.GetAsync<string>(nameof(selectedCategory));
            selectedCategory = stringResults.Success ? stringResults.Value : "All";

            stringResults = await ProtectedSessionStorage.GetAsync<string>(nameof(selectedStatus));
            selectedStatus = stringResults.Success ? stringResults.Value : "All";

            stringResults = await ProtectedSessionStorage.GetAsync<string>(nameof(searchText));
            searchText = stringResults.Success ? stringResults.Value : "";

            var boolResults = await ProtectedSessionStorage.GetAsync<bool>(nameof(isSortedByNew));
            isSortedByNew = boolResults.Success ? boolResults.Value : true;
        }

        /// <summary>
        /// Saves the filter States as key and value.
        /// </summary>
        /// <returns></returns>
        private async Task SaveFilterState()
        {
            await ProtectedSessionStorage.SetAsync(nameof(selectedCategory), selectedCategory);
            await ProtectedSessionStorage.SetAsync(nameof(selectedStatus), selectedStatus);
            await ProtectedSessionStorage.SetAsync(nameof(searchText), searchText);
            await ProtectedSessionStorage.SetAsync(nameof(isSortedByNew), isSortedByNew);

        }


        /// <summary>
        /// Only approved suggestions, which are visible to the user, are filtered then the filterd list is saved.
        /// </summary>
        /// <returns>filterd suggestions</returns>
        private async Task FilterSuggestions()
        {
            var output = await SuggestionData.GetAllApprovedSuggestions();

            //filters on the category name when is not all. selectedCategory matches the CategoryName of the approvedSuggestions
            if (selectedCategory != "All")
            {
                output = output.Where(s => s.Category?.CategoryName == selectedCategory).ToList();
            }
            //filters on the category name when is not all. selectedCategory matches the CategoryName of the approvedSuggestions
            if (selectedStatus != "All")
            {
                output = output.Where(s => s.SuggestionStatus?.StatusName == selectedStatus).ToList();
            }
            //Checks if the suggestion text or the description contains the searchText
            if (string.IsNullOrWhiteSpace(searchText) == false)
            {
                output = output.Where(
                    s => s.Suggestion.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                    s.Description.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            //if sortedbyNew is selected, sortes the list by DateCreated else orders by uservotes and when votes are equal it is orderd by DateCreated
            if (isSortedByNew)
            {
                output = output.OrderByDescending(s => s.DateCreated).ToList();
            }
            else
            {
                output = output.OrderByDescending(s => s.UserVotes.Count)
                                .ThenByDescending(s => s.DateCreated).ToList();

            }

            suggestions = output;
            await SaveFilterState();
        }
        /// <summary>
        /// Will be called when new is toggled. It enables as to run the FilterSuggestions() aagin after chenging the filtering
        /// </summary>
        /// <param name="isNew"></param>
        /// <returns></returns>
        private async Task OrderByNew(bool isNew)
        {
            isSortedByNew = isNew;
            await FilterSuggestions();
        }

        /// <summary>
        /// It filters when the user is typing
        /// </summary>
        /// <param name="SearchInput"></param>
        /// <returns></returns>
        private async Task OnSearchInput(string SearchInput)
        {
            searchText = SearchInput;
            await FilterSuggestions();
        }

        private async Task OnCategoryClick(string category = "All")
        {
            selectedCategory = category;
            await FilterSuggestions();
        }

        private async Task OnStatusClick(string status = "All")
        {
            selectedStatus = status;
            await FilterSuggestions();
        }
        /// <summary>
        /// Click to upvote when there are no upvotes or show the two digit number of upvotes if they exist.
        /// </summary>
        /// <param name="suggestion"></param>
        /// <returns></returns>
        private string GetUpvoteTopText(SuggestionModel suggestion)
        {
            if (suggestion.UserVotes?.Count > 0)
            {
                return suggestion.UserVotes.Count.ToString("00");
            }
            else
            {
                return "Click To";
            }
        }

        /// <summary>
        /// GetUpvoteBottomText() makes sure that we write upvotes only for more than one upvote
        /// </summary>
        /// <param name="suggestion"></param>
        /// <returns></returns>
        private string GetUpvoteBottomText(SuggestionModel suggestion)
        {
            if (suggestion.UserVotes?.Count > 1)
            {
                return "Upvotes";
            }
            else
            {
                return "Upvote";
            }
        }

        /// <summary>
        /// When a suggestion is clicked, the details page for that suggestion will be opened.The path of the suggestion will include the Id of the suggestion
        /// </summary>
        /// <param name="suggestion"></param>
        private void OpenDetails(SuggestionModel suggestion)
        {
            NavigationManager.NavigateTo($"/Details/{suggestion.Id}");
        }
    }
}