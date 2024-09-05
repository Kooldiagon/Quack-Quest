using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] UIText highscoreText;

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
}
