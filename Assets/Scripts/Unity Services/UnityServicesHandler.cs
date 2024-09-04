using Unity.Services.Core;

public class UnityServicesHandler : SingletonMonoBehaviour<UnityServicesHandler>
{
    private UserAuthentication userAuthentication;
    private RemoteConfig remoteConfig;

    public UserAuthentication UserAuthentication { get => userAuthentication; }
    public RemoteConfig RemoteConfig { get => remoteConfig; }

    public override void Awake()
    {
        base.Awake();
        InitialiseServices();
    }

    // Initialising all Unity Services in the project
    private async void InitialiseServices()
    {
        await UnityServices.InitializeAsync();

        userAuthentication = new UserAuthentication();
        await userAuthentication.Initialise();

        remoteConfig = new RemoteConfig();
        await remoteConfig.Initialise();

        EventHandler.Instance.ServicesInitialised();
    }
}
