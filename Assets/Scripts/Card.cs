using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Sprite front, back;
    private Image image;
    private Button button;

    void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    // Handles setting up the card values
    public void SetUp()
    {

    }

    // Called when the card is clicked
    public void Clicked()
    {
        StartCoroutine(Flip());
    }

    private IEnumerator Flip()
    {
        button.interactable = false;

        Sprite targetSprite = back;
        int direction = 1;
        if(transform.rotation.eulerAngles.y == -180)
        {
            targetSprite = front;
            direction = -1;
        }

        int totalFrames = 18;
        for (int f = 1; f <= totalFrames; f++)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + ((180f / totalFrames) * direction), 0);
            if (f == (totalFrames / 2))
            {
                image.sprite = targetSprite;
            }
            yield return new WaitForFixedUpdate();
        }
        button.interactable = true;
    }
}
