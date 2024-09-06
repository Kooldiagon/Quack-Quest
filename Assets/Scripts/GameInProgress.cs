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
    [JsonProperty("matchedCards"), SerializeField] private List<int> matchedCards = new List<int>();
    [JsonProperty("currentCardIndex"), SerializeField] private int currentCardIndex = -1;

    [JsonIgnore] public int Score { get => score; set => score = value; }
    [JsonIgnore] public int Combo { get => combo; set => combo = value; }
    [JsonIgnore] public int Health { get => health; set => health = value; }
    [JsonIgnore] public int Columns { get => columns; set => columns = value; }
    [JsonIgnore] public int Rows { get => rows; set => rows = value; }
    [JsonIgnore] public int GameSeed { get => gameSeed; set => gameSeed = value; }
    [JsonIgnore] public List<int> MatchedCards { get => matchedCards; set => matchedCards = value; }
    [JsonIgnore] public int CurrentCardIndex { get => currentCardIndex; set => currentCardIndex = value; }
}
