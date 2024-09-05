using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private Deck deck;
    private int score, combo, health, coloums, rows, cardsMatched;
    private Card currentCard;

    private NumberGenerator generator;

    public int Score { get => score; }
    public int Combo { get => combo; }
    public int Health { get => health; }
    public int Coloums { get => coloums; }
    public NumberGenerator Generator { get => generator; }
    public int Rows { get => rows; }

    private void OnEnable()
    {
        EventHandler.Instance.OnCardClicked += CheckCard;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnCardClicked -= CheckCard;
    }

    public void NewGame()
    {
        generator = new NumberGenerator((int)DateTime.UtcNow.Ticks);

        SetScore(0);
        SetCombo(0);
        SetHealth(5);
        coloums = 4;
        rows = 4;

        ShuffleCards();
    }

    public void CheckCard(Card card)
    {
        if (currentCard == null)
        {
            currentCard = card;
            return;
        }

        if (currentCard.CardID == card.CardID)
        {
            SetCombo(combo + 1);
            SetScore(score + combo);
            cardsMatched += 2;
            if (cardsMatched == coloums * rows)
            {
                NewLevel();
            }
        }
        else
        {
            currentCard.Flip();
            card.Flip();
            SetCombo(0);
            SetHealth(health - 1);
        }
        currentCard = null;
    }

    private void NewLevel()
    {
        if (coloums == rows)
        {
            coloums += 1;
        }
        else
        {
            rows += 1;
        }
        SetHealth(health + 2);
        ShuffleCards();
    }

    private void ShuffleCards()
    {
        cardsMatched = 0;
        currentCard = null;

        List<Sprite> possibleCards = new List<Sprite>(deck.Cards);
        List<Sprite> cards = new List<Sprite>();
        for (int c = 0; c < (int)Math.Floor((double)(coloums * rows) / 2); c++)
        {
            int index = generator.RandomInt(0, possibleCards.Count - 1);
            for (int i = 0; i < 2; i++)
            {
                cards.Add(possibleCards[index]);
            }
            possibleCards.RemoveAt(index);
        }

        EventHandler.Instance.CardsShuffled(cards);
    }

    private void SetScore(int newValue)
    {
        if (score == newValue) return;
        score = newValue;
        EventHandler.Instance.ScoreChanged();
    }

    private void SetCombo(int newValue)
    {
        if (combo == newValue) return;
        combo = newValue;
        EventHandler.Instance.ComboChanged();
    }

    private void SetHealth(int newValue)
    {
        if (health == newValue) return;
        health = newValue;
        EventHandler.Instance.HealthChanged();
    }
}
