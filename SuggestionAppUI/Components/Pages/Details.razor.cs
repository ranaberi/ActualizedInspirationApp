using Microsoft.AspNetCore.Components;
using SuggestionAppLibrary.DataAccess;
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
        private UserModel loggedInUser;

        protected async override Task OnInitializedAsync()
        {
            suggestion = await suggestionData.GetSuggestion(Id);
            loggedInUser = await authProvider.GetUserFromAuth(UserData);
        }
        private void ClosePage()
        {
            NavigationManager.NavigateTo("/");
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
                if (suggestion.Author.Id == loggedInUser?.Id)
                {
                    return "Awaiting";
                }
                else
                {
                    return "Click To";
                }
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

        private async Task VoteUp()
        {
            if (loggedInUser is not null)
            {
                if (suggestion.Author.Id == loggedInUser.Id)
                {
                    //Can't vote on your own suggestion
                    return;
                }
                if (suggestion.UserVotes.Add(loggedInUser.Id) == false)
                {
                    suggestion.UserVotes.Remove(loggedInUser.Id);
                }
                await suggestionData.UpvoteSuggestion(suggestion.Id, loggedInUser.Id);
            }
            else
            {
                NavigationManager.NavigateTo("/MicrosoftIdentity/Account/SignIn", true);
            }
        }
    }
}