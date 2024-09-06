using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameScreen : MonoBehaviour
{
    [SerializeField] private UIText scoreText, comboText, healthText;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private RectTransform infoRT, gridRT;

    void OnEnable()
    {
        EventHandler.Instance.OnScoreChanged += UpdateScore;
        EventHandler.Instance.OnComboChanged += UpdateCombo;
        EventHandler.Instance.OnHealthChanged += UpdateHealth;
        EventHandler.Instance.OnCardsShuffled += NewPile;
        EventHandler.Instance.OnHideCountdown += CountdownToHide;
    }

    void OnDisable()
    {
        EventHandler.Instance.OnScoreChanged -= UpdateScore;
        EventHandler.Instance.OnComboChanged -= UpdateCombo;
        EventHandler.Instance.OnHealthChanged -= UpdateHealth;
        EventHandler.Instance.OnCardsShuffled -= NewPile;
        EventHandler.Instance.OnHideCountdown += CountdownToHide;
    }

    private void NewPile(List<Sprite> cards)
    {
        UIHandler.Instance.ClearChildren(gridRT);
        float width = (float)Screen.width + gridRT.sizeDelta.x, height = (float)Screen.height + gridRT.sizeDelta.y;
        float cellSize = Mathf.Min((width - (grid.spacing.x * (GameManager.Instance.GameProgress.Columns - 1))) / GameManager.Instance.GameProgress.Columns, (height - (grid.spacing.y * (GameManager.Instance.GameProgress.Rows - 1))) / GameManager.Instance.GameProgress.Rows);
        grid.cellSize = Vector2.one * cellSize;
        grid.constraintCount = GameManager.Instance.GameProgress.Columns;

        int i = 0;
        while (cards.Count > 0)
        {
            int index = GameManager.Instance.Generator.RandomInt(0, cards.Count - 1);
            Card card = ObjectPool.Instance.Spawn(cardPrefab, grid.transform).GetComponent<Card>();
            card.SetUp(cards[index], i);
            cards.RemoveAt(index);
            GameManager.Instance.OrderedCards.Add(card);
            i++;
        }

        UpdateScore();
        UpdateCombo();
        UpdateHealth();
    }

    private void CountdownToHide()
    {
        StartCoroutine(FlipAllCards());
    }

    private IEnumerator FlipAllCards()
    {
        yield return new WaitForSeconds(gridRT.childCount * GameManager.Instance.GameData.TimePerCard);

        foreach (Transform child in gridRT)
        {
            child.GetComponent<Card>().Flip();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
        }
    }

    private void UpdateScore()
    {
        scoreText.SetText($"{GameManager.Instance.GameProgress.Score}");
        UIHandler.Instance.Refresh(infoRT);
    }

    private void UpdateCombo()
    {
        comboText.SetText($"{GameManager.Instance.GameProgress.Combo}");
        UIHandler.Instance.Refresh(infoRT);
    }

    private void UpdateHealth()
    {
        healthText.SetText($"{GameManager.Instance.GameProgress.Health}");
        UIHandler.Instance.Refresh(infoRT);
    }

    public void PauseButton()
    {
        UIHandler.Instance.ShowPopUp("Are you sure you want to pause your run and return to the home screen?\n\nYou can resume your run at a later date.", AcceptPB, DeclinePB);
    }

    private void AcceptPB()
    {
        GameManager.Instance.SaveProgress();
        GameManager.Instance.ReturnHome();
    }

    private void DeclinePB()
    {

    }
}
