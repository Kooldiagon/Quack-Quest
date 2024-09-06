using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : SingletonMonoBehaviour<UIHandler>
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private PopUp popUp;
    private GameObject currentScreen;

    private ThemeConfig theme;

    public ThemeConfig Theme { get => theme; }

    public void LoadUI()
    {
        theme = UnityServicesHandler.Instance.RemoteConfig.Json<ThemeConfig>("Colour Theme");
        ChangeScreen(startScreen);
    }

    public void ShowPopUp(string messageText, Action _acceptAction, Action _declineAction)
    {
        popUp.gameObject.SetActive(true);
        popUp.SetUp(messageText, _acceptAction, _declineAction);
    }

    public void ChangeScreen(GameObject newScreen)
    {
        if (currentScreen != null)
        {
            currentScreen.SetActive(false);
        }

        currentScreen = newScreen;
        newScreen.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    // Remove all child objects
    public void ClearChildren(RectTransform rectTransform)
    {
        while (rectTransform.childCount != 0)
        {
            ObjectPool.Instance.Remove(rectTransform.GetChild(0).gameObject);
        }
    }

    // Refreshes a rect transform so that the content size fitter is correct
    public void Refresh(RectTransform rectTransform)
    {
        StartCoroutine(DelayRefresh(rectTransform));
    }

    // Delays refresh to ensure all changes from the frame are accounted for
    private IEnumerator DelayRefresh(RectTransform rectTransform)
    {
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}
