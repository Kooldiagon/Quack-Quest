using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    [JsonProperty("soundVolume"), SerializeField] private int soundVolume = 100; // Sound volume level
    [JsonProperty("musicVolume"), SerializeField] private int musicVolume = 100; // Music volume level
    [JsonProperty("highscore"), SerializeField] private int highscore = 0; // Player's highest score

    [JsonIgnore] public int SoundVolume { get => soundVolume; set => soundVolume = value; }
    [JsonIgnore] public int MusicVolume { get => musicVolume; set => musicVolume = value; }
    [JsonIgnore] public int Highscore { get => highscore; set => highscore = value; }
}
