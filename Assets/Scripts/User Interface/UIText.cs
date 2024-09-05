using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    [SerializeField] private QuackQuest.Theme fontColour;
    [SerializeField] private float maxHeight, maxWidth;

    private TextMeshProUGUI tmp;
    private RectTransform rt;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
        tmp.color = UIHandler.Instance.Theme.Colours(fontColour);
    }

    // Sets the text and adjusts the text box's width
    public void SetText(string text)
    {
        tmp.text = text;
        tmp.ForceMeshUpdate();

        float newWidth = tmp.preferredWidth;
        if (maxWidth != 0) newWidth = Mathf.Min(tmp.preferredWidth, maxWidth);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

        float newHeight = tmp.preferredHeight;
        if (maxHeight != 0) newHeight = Mathf.Min(tmp.preferredHeight, maxHeight);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
    }
}
