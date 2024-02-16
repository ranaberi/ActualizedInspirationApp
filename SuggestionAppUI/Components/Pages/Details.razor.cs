using Microsoft.AspNetCore.Components;
using SuggestionAppLibrary.DataAccess;
using System;
using System.Collections.Generic;
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

        private List<StatusModel> statuses;
        private string settingStatus = "";
        private string urlText = "";


        protected async override Task OnInitializedAsync()
        {
            suggestion = await suggestionData.GetSuggestion(Id);
            loggedInUser = await authProvider.GetUserFromAuth(UserData);
            statuses = await statusData.GetAllStatuses();

        }

        private async Task CompleteSetStatus()
        {
            switch (settingStatus)
            {
                case "completed":
                    if (string.IsNullOrWhiteSpace(urlText))
                    {
                        return;
                    }
                    suggestion.SuggestionStatus = statuses.Where(s => s.StatusName.ToLower() == settingStatus.ToLower()).First();
                    suggestion.OwnerNotes = $"This is an inspiring suggestion. You can find a related one here: <a href='{urlText}' target='_blank'> {urlText} </a>";
                    break;

                case "watching":
                    if (string.IsNullOrWhiteSpace(urlText))
                    {
                        return;
                    }
                    suggestion.SuggestionStatus = statuses.Where(s => s.StatusName.ToLower() == settingStatus.ToLower()).First();
                    suggestion.OwnerNotes = $"We noticed the interest this suggestion is getting! If more people are interested we will address the next steps to realise it.";
                    break;
                case "upcoming":
                    if (string.IsNullOrWhiteSpace(urlText))
                    {
                        return;
                    }
                    suggestion.SuggestionStatus = statuses.Where(s => s.StatusName.ToLower() == settingStatus.ToLower()).First();
                    suggestion.OwnerNotes = $"Great suggestion! We have a group formed to address this topic";
                    break;
                case "dismissed":
                    if (string.IsNullOrWhiteSpace(urlText))
                    {
                        return;
                    }
                    suggestion.SuggestionStatus = statuses.Where(s => s.StatusName.ToLower() == settingStatus.ToLower()).First();
                    suggestion.OwnerNotes = $"Sometimes a good idea doesn't fit within our scope and vision. This is one of those ideas.";
                    break;
                default:
                    return;
            }
            settingStatus = null;
            await suggestionData.UpdateSuggestion(suggestion);
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
        private string GetVoteClass()
        {
            if (suggestion.UserVotes is null || suggestion.UserVotes.Count == 0)
            {
                return "suggestion-detail-no-votes";
            }
            else if (suggestion.UserVotes.Contains(loggedInUser?.Id))
            {
                return "suggestion-detail-voted";
            }
            else
            {
                return "suggestion-detail-not-voted";
            }
        }

        private string GetStatusClass()
        {
            if (suggestion is null || suggestion.SuggestionStatus is null)
            {
                return "suggestion-detail-status-none";
            }
            string output = suggestion.SuggestionStatus.StatusName switch
            {
                "Completed" => "suggestion-detail-status-completed",
                "Watching" => "suggestion-detail-status-watching",
                "Upcoming" => "suggestion-detail-status-upcoming",
                "Dismissed" => "suggestion-detail-status-dismissed",
                _ => "suggestion-detail-status-none",
            };
            return output;
        }
    }
}