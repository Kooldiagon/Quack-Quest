using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private Deck deck;
    [SerializeField] private GameObject home;
    [SerializeField] private AudioClip match, mismatch, gameOver;
    private int score, combo, health, coloums, rows, cardsMatched, gameSeed;
    private Card currentCard;
    private GameConfig gameData;
    private NumberGenerator generator;
    [SerializeField] private SaveData saveData;

    public int Score { get => score; }
    public int Combo { get => combo; }
    public int Health { get => health; }
    public int Coloums { get => coloums; }
    public int Rows { get => rows; }
    public NumberGenerator Generator { get => generator; }
    public GameConfig GameData { get => gameData; }
    public SaveData SaveData { get => saveData; }

    private void OnEnable()
    {
        EventHandler.Instance.OnCardClicked += CheckCard;
        EventHandler.Instance.OnServicesInitialised += LoadData;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnCardClicked -= CheckCard;
    }

    public void LoadData()
    {
        EventHandler.Instance.OnServicesInitialised -= LoadData;
        Task.Run(async () =>
        {
            saveData = await UnityServicesHandler.Instance.CloudSave.Load<SaveData>("SaveData");
        }).ContinueWith(task =>
        {
            UIHandler.Instance.LoadUI();
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void NewGame()
    {
        gameData = UnityServicesHandler.Instance.RemoteConfig.Json<GameConfig>("Game Data");
        gameSeed = (int)DateTime.UtcNow.Ticks;
        generator = new NumberGenerator(gameSeed);

        SetScore(gameData.StartScore);
        SetCombo(gameData.StartCombo);
        SetHealth(gameData.StartHealth);
        coloums = gameData.StartColumn;
        rows = gameData.StartRow;

        ShuffleCards();
    }

    public void CheckCard(Card card)
    {
        if (health == 0) return;

        if (currentCard == null)
        {
            currentCard = card;
            return;
        }

        if (currentCard.CardID == card.CardID)
        {
            AudioHandler.Instance.PlaySound(match);
            SetCombo(combo + 1);
            SetScore(score + combo);
            cardsMatched++;
            if (cardsMatched == (int)Math.Floor((double)(coloums * rows) / 2))
            {
                NewLevel();
            }
        }
        else
        {
            AudioHandler.Instance.PlaySound(mismatch);
            currentCard.Flip();
            card.Flip();
            SetCombo(gameData.StartCombo);
            SetHealth(health - 1);
        }
        currentCard = null;
    }

    private void IncreaseCards()
    {
        // Exiting if already maxed out
        if (coloums == gameData.MaxColumns && rows == gameData.MaxRows)
        {
            return;
        }

        if (coloums == gameData.MaxColumns)
        {
            rows += gameData.NewLevelCardIncrease;
            return;
        }

        if (rows == gameData.MaxRows)
        {
            coloums += gameData.NewLevelCardIncrease;
            return;
        }

        // Alternating
        if (coloums == rows)
        {
            coloums += gameData.NewLevelCardIncrease;
        }
        else
        {
            rows += gameData.NewLevelCardIncrease;
        }
    }

    private void NewLevel()
    {
        gameSeed = (int)DateTime.UtcNow.Ticks;
        generator = new NumberGenerator(gameSeed);
        IncreaseCards();
        SetHealth(health + gameData.NewLevelHealthGain);
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
        if (health == 0) UIHandler.Instance.ShowPopUp(GameEnded(), ReturnHome, null);
    }

    private string GameEnded()
    {
        AudioHandler.Instance.PlaySound(gameOver);
        string goText = "Game Over\n\n";

        if (score > saveData.Highscore)
        {
            goText += "New Highscore\n";
            saveData.Highscore = score;
            UnityServicesHandler.Instance.CloudSave.Save("SaveData", saveData);
        }
        else
        {
            goText += "Score\n";
        }

        goText += $"{score}";
        return goText;
    }

    public void ReturnHome()
    {
        UIHandler.Instance.ChangeScreen(home);
    }
}
