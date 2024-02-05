using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuggestionAppUI.Components.Pages
{
    public partial class Details
    {

        [Parameter]
        public string Id { get; set; }
        private SuggestionModel suggestion;

        protected async override Task OnInitializedAsync()
        {
            suggestion = await suggestionData.GetSuggestion(Id);
        }
        private void ClosePage()
        {
            navManager.NavigateTo("/");
        }

        /// <summary>
        /// Click to upvote when there are no upvotes or show the two digit number of upvotes if they exist.
        /// </summary>
        /// <returns></returns>
        private string GetUpvoteTopText()
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
        /// <returns></returns>
        private string GetUpvoteBottomText()
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
    }
}