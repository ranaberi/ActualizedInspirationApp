using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace SuggestionAppUI.Components.Pages
{
    public partial class Error
    {
        [CascadingParameter]
        private Microsoft.AspNetCore.Http.HttpContext? HttpContext { get; set; }

        private string? RequestId { get; set; }
        private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        protected override void OnInitialized() =>
            RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
    }
}