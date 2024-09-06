using System;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : Singleton<EventHandler>
{
    public event Action OnServicesInitialised;
    public void ServicesInitialised() // Called when the unity services finish initialising
    {
        OnServicesInitialised?.Invoke();
    }

    #region Gameplay Events

    public event Action OnScoreChanged;
    public void ScoreChanged() // Called when the score's value is changed
    {
        OnScoreChanged?.Invoke();
    }

    public event Action OnComboChanged;
    public void ComboChanged() // Called when the combo's value is changed
    {
        OnComboChanged?.Invoke();
    }

    public event Action OnHealthChanged;
    public void HealthChanged() // Called when the health's value is changed
    {
        OnHealthChanged?.Invoke();
    }

    public event Action<List<Sprite>> OnCardsShuffled;
    public void CardsShuffled(List<Sprite> cards) // Called when a new pile of cards have been determined
    {
        OnCardsShuffled?.Invoke(cards);
    }

    public event Action<Card> OnCardClicked;
    public void CardClicked(Card card) // Called when the player interacts with a card
    {
        OnCardClicked?.Invoke(card);
    }

    public event Action OnHideCountdown;
    public void HideCountdown() // Called when starting the countdown to hide all cards
    {
        OnHideCountdown?.Invoke();
    }

    #endregion
}
