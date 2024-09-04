using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class ThemeConfig
{
    [JsonProperty("colours"), SerializeField] private SerializableDictionary<QuackQuest.Theme, string> colours; // The possible colours

    public Color Colours(QuackQuest.Theme index)
    {
        Color colour = Color.white;
        if (!ColorUtility.TryParseHtmlString(colours[index], out colour))
        {
            Debug.LogError("Colour failed");
        }

        return colour;
    }
}
