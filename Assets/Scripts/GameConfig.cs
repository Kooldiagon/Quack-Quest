using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameConfig
{
    [JsonProperty("startScore"), SerializeField] private int startScore;
    [JsonProperty("startCombo"), SerializeField] private int startCombo;
    [JsonProperty("startHealth"), SerializeField] private int startHealth;
    [JsonProperty("startColumn"), SerializeField] private int startColumn;
    [JsonProperty("startRow"), SerializeField] private int startRow;
    [JsonProperty("newLevelCardIncrease"), SerializeField] private int newLevelCardIncrease;
    [JsonProperty("newLevelHealthGain"), SerializeField] private int newLevelHealthGain;

    [JsonProperty("maxColumns"), SerializeField] private int maxColumns;
    [JsonProperty("maxRows"), SerializeField] private int maxRows;

    [JsonProperty("timePerCard"), SerializeField] private float timePerCard;
    [JsonProperty("cardAnimationFrames"), SerializeField] private int cardAnimationFrames;

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
