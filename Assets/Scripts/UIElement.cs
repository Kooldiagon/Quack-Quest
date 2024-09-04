using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class UIElement : MonoBehaviour
{
    [SerializeField] private QuackQuest.Theme elementType;

    private Graphic graphic;

    private void Awake()
    {
        graphic = GetComponent<Graphic>();
        ChangeColour(elementType);
    }

    public void ChangeColour(QuackQuest.Theme type)
    {
        graphic.color = UIHandler.Instance.Theme.Colours(type);
    }
}
