using System;

public class EventHandler : Singleton<EventHandler>
{
    public event Action OnServicesInitialised;
    public void ServicesInitialised() // Called when the unity services finish initialising
    {
        OnServicesInitialised?.Invoke();
    }
}
