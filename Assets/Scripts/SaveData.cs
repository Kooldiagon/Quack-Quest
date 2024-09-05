using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    [JsonProperty("soundVolume"), SerializeField] private int soundVolume = 100; // Sound volume level
    [JsonProperty("musicVolume"), SerializeField] private int musicVolume = 100; // Music volume level
    [JsonProperty("highscore"), SerializeField] private int highscore = 0; // Player's highest score
    [JsonProperty("highscore"), SerializeField] private GameInProgress existingGame = null; // Contains the game data if the player left

    [JsonIgnore] public int SoundVolume { get => soundVolume; set => soundVolume = value; }
    [JsonIgnore] public int MusicVolume { get => musicVolume; set => musicVolume = value; }
    [JsonIgnore] public int Highscore { get => highscore; set => highscore = value; }
    [JsonIgnore] public GameInProgress ExistingGame { get => existingGame; set => existingGame = value; }
}
