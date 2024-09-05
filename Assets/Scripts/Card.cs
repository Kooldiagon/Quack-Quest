using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private AudioClip flipAudio;
    [SerializeField] private Sprite back;
    private Sprite front;
    private Image image;
    private Button button;
    private string cardID;

    public string CardID { get => cardID; }

    void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    // Handles setting up the card values
    public void SetUp(Sprite _front)
    {
        front = _front;
        image.sprite = front;
        cardID = _front.name;
        button.interactable = false;
        transform.localScale = Vector3.one;
    }

    // Called when the card is clicked by the player
    public void PlayerFlip()
    {
        StartCoroutine(FlipAnimation(true));
    }

    public void Flip()
    {
        StartCoroutine(FlipAnimation(false));
    }

    private IEnumerator FlipAnimation(bool playerInteraction)
    {
        button.interactable = false;

        Sprite targetSprite = back;
        float startScale = transform.localScale.x, endScale = -1f;
        if(startScale <= -1)
        {
            targetSprite = front;
            endScale = 1f;
        }

        if (playerInteraction) AudioHandler.Instance.PlaySound(flipAudio);
        for (float f = 1; f <= GameManager.Instance.GameData.CardAnimationFrames; f++)
        {
            transform.localScale = new Vector3(Mathf.Lerp(startScale, endScale, f / GameManager.Instance.GameData.CardAnimationFrames), 1, 1);
            if (f == (GameManager.Instance.GameData.CardAnimationFrames / 2))
            {
                image.sprite = targetSprite;
            }
            yield return new WaitForFixedUpdate();
        }

        if (playerInteraction) EventHandler.Instance.CardClicked(this);
        if (image.sprite == back) button.interactable = true;

    }
}
