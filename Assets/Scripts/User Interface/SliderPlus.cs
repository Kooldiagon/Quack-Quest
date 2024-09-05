using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderPlus : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private UIText value;
    [SerializeField] private string suffix;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite defaultIcon, zeroIcon;

    public Slider Slider { get => slider; }

    public void SetValue(float value)
    {
        slider.SetValueWithoutNotify(value);
        ValueChanged();
    }

    public void ValueChanged()
    {
        value.SetText($"{slider.value}{suffix}");
        if (slider.value != slider.minValue)
        {
            icon.sprite = defaultIcon;
        }
        else
        {
            icon.sprite = zeroIcon;
        }
    }
}
