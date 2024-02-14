using System.Linq;
using System.Threading.Tasks;

namespace SuggestionAppUI.Helpers;

public static class AuthenticationStateProviderHelpers
{
    public static async Task<UserModel> GetUserFromAuth(this AuthenticationStateProvider authStateProvider, IUserData userData)
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();
        string objectId = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;
        return await userData.GetUserFromAuthentication(objectId);
    }
}
