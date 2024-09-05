using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Custom/Deck", order = 1)]
public class Deck : ScriptableObject
{
    [SerializeField] private List<Sprite> cards; // Possible cards

    public List<Sprite> Cards { get => cards; }
}
