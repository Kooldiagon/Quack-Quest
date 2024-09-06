using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private Deck deck;
    [SerializeField] private GameObject home;
    [SerializeField] private AudioClip match, mismatch, gameOver;

    private GameInProgress gameProgress;

    private Card currentCard;
    private GameConfig gameData;
    private NumberGenerator generator;
    private SaveData saveData;
    private List<Card> orderedCards;

    public NumberGenerator Generator { get => generator; }
    public GameConfig GameData { get => gameData; }
    public SaveData SaveData { get => saveData; set => saveData = value; }
    public GameInProgress GameProgress { get => gameProgress; }
    public List<Card> OrderedCards { get => orderedCards; }

    private void OnEnable()
    {
        EventHandler.Instance.OnCardClicked += CheckCard;
        EventHandler.Instance.OnServicesInitialised += Load;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnCardClicked -= CheckCard;
    }

    // Starts the game after Unity Services have finished initialising
    public void Load()
    {
        EventHandler.Instance.OnServicesInitialised -= Load;
        UIHandler.Instance.LoadUI();
        AudioHandler.Instance.LoadAudio();
        gameData = UnityServicesHandler.Instance.RemoteConfig.Json<GameConfig>("Game Data");
    }

    // Called when the user loads an existing run
    public void LoadGame()
    {
        currentCard = null;
        gameProgress = saveData.ExistingGame;
        generator = new NumberGenerator(gameProgress.GameSeed);
        ShuffleCards();
        foreach (Card card in orderedCards)
        {
            // Checks that the card has not already been matched
            if (!gameProgress.MatchedCards.Contains(card.PositionIndex))
            {
                card.InstantFlip(-1, false);
            }
        }
        if(gameProgress.CurrentCardIndex != -1) orderedCards[gameProgress.CurrentCardIndex].InstantFlip(1, true);
    }

    // Called when the user starts a new run
    public void NewGame()
    {
        gameProgress = new GameInProgress();
        gameProgress.GameSeed = (int)DateTime.UtcNow.Ticks;
        generator = new NumberGenerator(gameProgress.GameSeed);

        SetScore(gameData.StartScore);
        SetCombo(gameData.StartCombo);
        SetHealth(gameData.StartHealth);
        gameProgress.Columns = gameData.StartColumn;
        gameProgress.Rows = gameData.StartRow;
        gameProgress.MatchedCards = new List<int>();
        currentCard = null;

        ShuffleCards();
        EventHandler.Instance.HideCountdown();
    }

    // Called to check for match
    public void CheckCard(Card card)
    {
        if (gameProgress.Health == 0) return; // Prevents queued card flips from being actioned

        // No other card to compare with
        if (currentCard == null)
        {
            currentCard = card;
            return;
        }

        if (currentCard.CardID == card.CardID)
        {
            // Match
            AudioHandler.Instance.PlaySound(match);
            SetCombo(gameProgress.Combo + 1);
            SetScore(gameProgress.Score + gameProgress.Combo);
            gameProgress.MatchedCards.Add(currentCard.PositionIndex);
            gameProgress.MatchedCards.Add(card.PositionIndex);
            if (gameProgress.MatchedCards.Count == orderedCards.Count)
            {
                NewLevel();
            }
        }
        else
        {
            // Mismatch
            AudioHandler.Instance.PlaySound(mismatch);
            currentCard.Flip();
            card.Flip();
            SetCombo(gameData.StartCombo);
            SetHealth(gameProgress.Health - 1);
        }
        currentCard = null;
    }

    private void IncreaseCards()
    {
        // Exiting if already maxed out
        if (gameProgress.Columns == gameData.MaxColumns && gameProgress.Rows == gameData.MaxRows)
        {
            return;
        }

        if (gameProgress.Columns == gameData.MaxColumns)
        {
            gameProgress.Rows += gameData.NewLevelCardIncrease;
            return;
        }

        if (gameProgress.Rows == gameData.MaxRows)
        {
            gameProgress.Columns += gameData.NewLevelCardIncrease;
            return;
        }

        // Alternating
        if (gameProgress.Columns == gameProgress.Rows)
        {
            gameProgress.Columns += gameData.NewLevelCardIncrease;
        }
        else
        {
            gameProgress.Rows += gameData.NewLevelCardIncrease;
        }
    }

    // Creates a new level, called after the player completes a level
    private void NewLevel()
    {
        gameProgress.GameSeed = (int)DateTime.UtcNow.Ticks;
        generator = new NumberGenerator(gameProgress.GameSeed);
        IncreaseCards();
        SetHealth(gameProgress.Health + gameData.NewLevelHealthGain);
        gameProgress.MatchedCards = new List<int>();
        currentCard = null;

        ShuffleCards();
        EventHandler.Instance.HideCountdown();
    }

    // Assigns the cards which the player needs to match (does not handle assigning positions)
    private void ShuffleCards()
    {
        orderedCards = new List<Card>();
        List<Sprite> possibleCards = new List<Sprite>(deck.Cards);
        List<Sprite> cards = new List<Sprite>();
        for (int c = 0; c < (int)Math.Floor((double)(gameProgress.Columns * gameProgress.Rows) / 2); c++)
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
        if (gameProgress.Score == newValue) return;
        gameProgress.Score = newValue;
        EventHandler.Instance.ScoreChanged();
    }

    private void SetCombo(int newValue)
    {
        if (gameProgress.Combo == newValue) return;
        gameProgress.Combo = newValue;
        EventHandler.Instance.ComboChanged();
    }

    private void SetHealth(int newValue)
    {
        if (gameProgress.Health == newValue) return;
        gameProgress.Health = newValue;
        EventHandler.Instance.HealthChanged();
        if (gameProgress.Health == 0) UIHandler.Instance.ShowPopUp(GameEnded(), ReturnHome, null);
    }

    private string GameEnded()
    {
        AudioHandler.Instance.PlaySound(gameOver);
        string goText = "Game Over\n\n";

        if (gameProgress.Score > saveData.Highscore)
        {
            goText += "New Highscore\n";
            saveData.Highscore = gameProgress.Score;
            saveData.ExistingGame = new GameInProgress();
            saveData.InProgress = false;
            UnityServicesHandler.Instance.CloudSave.Save();
        }
        else
        {
            goText += "Score\n";
        }

        goText += $"{gameProgress.Score}";
        return goText;
    }

    public void SaveProgress()
    {
        if (currentCard != null)
        {
            gameProgress.CurrentCardIndex = currentCard.PositionIndex;
        }
        else
        {
            gameProgress.CurrentCardIndex = -1;
        }

        saveData.InProgress = true;
        saveData.ExistingGame = gameProgress;
        UnityServicesHandler.Instance.CloudSave.Save();
    }

    public void ReturnHome()
    {
        UIHandler.Instance.ChangeScreen(home);
    }

    private void OnApplicationQuit()
    {
        SaveProgress();
    }
}
