using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : SingletonMonoBehaviour<UIHandler>
{
    [SerializeField]
    private GameObject startScreen;
    private GameObject currentScreen;

    private ThemeConfig theme;

    public ThemeConfig Theme { get => theme; }

    private void OnEnable()
    {
        EventHandler.Instance.OnServicesInitialised += LoadUI;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnServicesInitialised -= LoadUI;
    }

    private void LoadUI()
    {
        theme = UnityServicesHandler.Instance.RemoteConfig.Json<ThemeConfig>("Colour Theme");
        ChangeScreen(startScreen);
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

    public void ClearChildren(RectTransform rectTransform)
    {
        while (rectTransform.childCount != 0)
        {
            ObjectPool.Instance.Remove(rectTransform.GetChild(0).gameObject);
        }
    }

    public void Refresh(RectTransform rectTransform)
    {
        StartCoroutine(DelayRefresh(rectTransform));
    }

    private IEnumerator DelayRefresh(RectTransform rectTransform)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}
