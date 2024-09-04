using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : SingletonMonoBehaviour<UIHandler>
{
    [SerializeField]
    private GameObject startScreen;

    private ThemeConfig theme;

    public ThemeConfig Theme { get => theme; }

    private void OnEnable()
    {
        EventHandler.Instance.OnServicesInitialised += ShowScreen;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnServicesInitialised -= ShowScreen;
    }

    private void ShowScreen()
    {
        theme = UnityServicesHandler.Instance.RemoteConfig.Json<ThemeConfig>("Colour Theme");
        startScreen.SetActive(true);
    }
}
