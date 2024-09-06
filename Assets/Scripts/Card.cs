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
    private int positionIndex;

    public string CardID { get => cardID; }
    public int PositionIndex { get => positionIndex; }

    void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    // Handles setting up the card values
    public void SetUp(Sprite _front, int _positionIndex)
    {
        front = _front;
        positionIndex = _positionIndex;
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

    // Target Scale 1 = Front
    public void InstantFlip(float targetScale, bool playerInteraction)
    {
        transform.localScale = new Vector3(targetScale, 1, 1);
        Sprite targetSprite = back;
        if (targetScale == 1)
        {
            targetSprite = front;
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
        image.sprite = targetSprite;

        if (playerInteraction) EventHandler.Instance.CardClicked(this);
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
