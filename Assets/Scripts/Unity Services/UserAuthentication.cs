using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class UserAuthentication
{
    public async Task Initialise()
    {
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };

        if (AuthenticationService.Instance.SessionTokenExists)
        {
            await Call(SignInCachedUser());
        }
        else
        {
            await Call(AnonymousSignIn());
        }
    }

    #region Anonymous

    public async Task AnonymousSignIn()
    {
        await Call(AuthenticationService.Instance.SignInAnonymouslyAsync());
    }

    #endregion

    public bool IsSignedIn()
    {
        return AuthenticationService.Instance.IsSignedIn;
    }

    public bool IsAuthorized()
    {
        return AuthenticationService.Instance.IsAuthorized;
    }

    public bool IsExpired()
    {
        return AuthenticationService.Instance.IsExpired;
    }

    public async Task DeleteAccount()
    {
        await Call(AuthenticationService.Instance.DeleteAccountAsync());
    }

    public async Task SignInCachedUser()
    {
        if (!AuthenticationService.Instance.SessionTokenExists)
        {
            return;
        }

        await Call(AuthenticationService.Instance.SignInAnonymouslyAsync());
    }

    private async Task Call(Task action)
    {
        try
        {
            await action;
        }
        catch (AuthenticationException e)
        {
            Debug.LogError(e);
        }
        catch (RequestFailedException e)
        {
            Debug.LogError(e);
        }
    }
}