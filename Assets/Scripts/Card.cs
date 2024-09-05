using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
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

        float totalFrames = 20;
        for (float f = 1; f <= totalFrames; f++)
        {
            transform.localScale = new Vector3(Mathf.Lerp(startScale, endScale, f / totalFrames), 1, 1);
            if (f == (totalFrames / 2))
            {
                image.sprite = targetSprite;
            }
            yield return new WaitForFixedUpdate();
        }

        if (playerInteraction) EventHandler.Instance.CardClicked(this);
        if (image.sprite == back) button.interactable = true;

    }
}
