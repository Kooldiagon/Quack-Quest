using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private UIText highscoreText;
    [SerializeField] private GameObject gameScreen;

    private void OnEnable()
    {
        if (GameManager.Instance.SaveData.Highscore == 0)
        {
            highscoreText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            highscoreText.transform.parent.gameObject.SetActive(true);
            highscoreText.SetText($"{GameManager.Instance.SaveData.Highscore}");
        }
    }

    public void PlayButton()
    {
        if (!GameManager.Instance.SaveData.InProgress)
        {
            UIHandler.Instance.ChangeScreen(gameScreen);
            GameManager.Instance.NewGame();
        }
        else
        {
            UIHandler.Instance.ShowPopUp("You have a game which is already in progress. Would you like to resume it?", AcceptResume, DeclineResume);
        }
    }

    private void AcceptResume()
    {
        UIHandler.Instance.ChangeScreen(gameScreen);
        GameManager.Instance.LoadGame();
    }

    private void DeclineResume()
    {
        UIHandler.Instance.ChangeScreen(gameScreen);
        GameManager.Instance.SaveData.ExistingGame = new GameInProgress();
        UnityServicesHandler.Instance.CloudSave.Save();
        GameManager.Instance.NewGame();
    }
}
