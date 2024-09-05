using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class GameConfig
{
    [JsonProperty("startScore"), SerializeField] private int startScore; // Starting score for a new game
    [JsonProperty("startCombo"), SerializeField] private int startCombo; // Starting combo for a new game
    [JsonProperty("startHealth"), SerializeField] private int startHealth; // Starting health for a new game
    [JsonProperty("startColumn"), SerializeField] private int startColumn; // The number of card columns at the start of the game
    [JsonProperty("startRow"), SerializeField] private int startRow; // The number of card rows at the start of the game
    [JsonProperty("newLevelCardIncrease"), SerializeField] private int newLevelCardIncrease; // Number of columns or rows to add upon a new level
    [JsonProperty("newLevelHealthGain"), SerializeField] private int newLevelHealthGain; // The health to give to the player upon a new level

    [JsonProperty("maxColumns"), SerializeField] private int maxColumns; // Max card columns
    [JsonProperty("maxRows"), SerializeField] private int maxRows; // Max card rows

    [JsonProperty("timePerCard"), SerializeField] private float timePerCard; // The time to show per card at the start of a level
    [JsonProperty("cardAnimationFrames"), SerializeField] private int cardAnimationFrames; // The number of Fixed Updates the flip animation takes

    [JsonIgnore] public int StartScore { get => startScore; }
    [JsonIgnore] public int StartCombo { get => startCombo; }
    [JsonIgnore] public int StartHealth { get => startHealth; }
    [JsonIgnore] public int StartColumn { get => startColumn; }
    [JsonIgnore] public int StartRow { get => startRow; }
    [JsonIgnore] public int NewLevelCardIncrease { get => newLevelCardIncrease; }
    [JsonIgnore] public int NewLevelHealthGain { get => newLevelHealthGain; }
    [JsonIgnore] public int MaxColumns { get => maxColumns; }
    [JsonIgnore] public int MaxRows { get => maxRows; }
    [JsonIgnore] public float TimePerCard { get => timePerCard; }
    [JsonIgnore] public int CardAnimationFrames { get => cardAnimationFrames; }
}
