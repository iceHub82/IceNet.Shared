using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using IceNet.Service.Preferences;

namespace IceNet.Shared;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IPreferenceStoreService _preferenceStoreService;

    public CustomAuthStateProvider(IPreferenceStoreService preferenceStoreService) => _preferenceStoreService = preferenceStoreService;

    private ClaimsPrincipal currentUser = new(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_preferenceStoreService.Exists("User"))
        {
            if (!currentUser.Identity.IsAuthenticated)
            {
                var userEmail = _preferenceStoreService.Get<string>("User");

                var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userEmail),
                    //new Claim( ClaimTypes.Role, "userRole")
                }, "auth"));

                return Task.FromResult(new AuthenticationState(authenticatedUser));
            }
        }

        return Task.FromResult(new AuthenticationState(currentUser));
    }

    public Task LogInAsync(string email)
    {
        var loginTask = LogInAsyncCore(email);
        NotifyAuthenticationStateChanged(loginTask);

        return loginTask;

        async Task<AuthenticationState> LogInAsyncCore(string email)
        {
            var user = await LoginWithExternalProviderAsync(email);
            currentUser = user;

            return new AuthenticationState(currentUser);
        }
    }

    private Task<ClaimsPrincipal> LoginWithExternalProviderAsync(string email)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Email, email)
        }, "auth"));

        _preferenceStoreService.Set("User", email);

        return Task.FromResult(authenticatedUser);
    }

    public async Task Logout()
    {
        _preferenceStoreService.Delete("User");
        currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));
    }
}