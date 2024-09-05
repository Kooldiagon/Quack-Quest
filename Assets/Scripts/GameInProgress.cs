using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameInProgress
{
    [JsonProperty("score"), SerializeField] private int score;
    [JsonProperty("combo"), SerializeField] private int combo;
    [JsonProperty("health"), SerializeField] private int health;
    [JsonProperty("columns"), SerializeField] private int columns;
    [JsonProperty("rows"), SerializeField] private int rows;
    [JsonProperty("gameSeed"), SerializeField] private int gameSeed;
    [JsonProperty("matchedCards"), SerializeField] private List<int> matchedCards;
}
