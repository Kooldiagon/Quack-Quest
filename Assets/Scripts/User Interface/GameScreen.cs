using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameScreen : MonoBehaviour
{
    [SerializeField] private UIText scoreText, comboText, healthText;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GridLayoutGroup grid;

    void OnEnable()
    {
        EventHandler.Instance.OnScoreChanged += UpdateScore;
        EventHandler.Instance.OnComboChanged += UpdateCombo;
        EventHandler.Instance.OnHealthChanged += UpdateHealth;
        EventHandler.Instance.OnCardsShuffled += NewPile;
    }

    void OnDisable()
    {
        EventHandler.Instance.OnScoreChanged -= UpdateScore;
        EventHandler.Instance.OnComboChanged -= UpdateCombo;
        EventHandler.Instance.OnHealthChanged -= UpdateHealth;
        EventHandler.Instance.OnCardsShuffled -= NewPile;
    }

    private void NewPile(List<Sprite> cards)
    {
        UIHandler.Instance.ClearChildren(grid.GetComponent<RectTransform>());
        grid.constraintCount = GameManager.Instance.Coloums;

        while (cards.Count > 0)
        {
            int index = GameManager.Instance.Generator.RandomInt(0, cards.Count - 1);
            Card card = ObjectPool.Instance.Spawn(cardPrefab, grid.transform).GetComponent<Card>();
            card.SetUp(cards[index]);
            cards.RemoveAt(index);
        }
        StartCoroutine(FlipAllCards());
        UpdateScore();
        UpdateCombo();
        UpdateHealth();
    }

    private IEnumerator FlipAllCards()
    {
        yield return new WaitForSeconds(3f);

        foreach (Transform child in grid.GetComponent<RectTransform>())
        {
            child.GetComponent<Card>().Flip();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
        }
    }

    private void UpdateScore()
    {
        scoreText.SetText($"{GameManager.Instance.Score}");
        UIHandler.Instance.Refresh(grid.GetComponent<RectTransform>());
    }

    private void UpdateCombo()
    {
        comboText.SetText($"{GameManager.Instance.Combo}");
        UIHandler.Instance.Refresh(grid.GetComponent<RectTransform>());
    }

    private void UpdateHealth()
    {
        healthText.SetText($"{GameManager.Instance.Health}");
        UIHandler.Instance.Refresh(grid.GetComponent<RectTransform>());
    }
}
