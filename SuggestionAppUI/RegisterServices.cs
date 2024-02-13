using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace SuggestionAppUI;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
        builder.Services.AddMemoryCache();
        builder.Services.AddControllers().AddMicrosoftIdentityUI();

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
        {
            policy.RequireClaim("jobTitle", "Admin");
        });
        });

        //the DBConnection which has the connection to the mongo database as a singleton
        //One database connection for everybody to use instead of AddScoped which is a singleton per user
        builder.Services.AddSingleton<IDBConnection, DBConnection>();
        //AddSingleton used same as DBConnection, instead of AddTransient which creates new instance every time there is a call.
        //Because CategoryData has access to the cache and collection which are not unique and can be shared accross users
        //All the ones below rely on DBConnection and MemoryCache
        builder.Services.AddSingleton<ICategoryData, MongoCategoryData>();
        builder.Services.AddSingleton<IStatusData, MongoStatusData>();
        builder.Services.AddSingleton<IUserData, MongoUserData>();
        builder.Services.AddSingleton<ISuggestionData, MongoSuggestionData>();
        builder.Services.AddSingleton<IUserData, MongoUserData>();
    }
}