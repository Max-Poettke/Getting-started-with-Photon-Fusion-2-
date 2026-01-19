using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyState : EntityState
{
    public DeckData deck;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    protected override void InitializeUI()
    {
        healthSlider.maxValue = MaxHealth;
        healthSlider.value = Health;
        healthText.text = Health.ToString();
    }

    protected override void UpdateUI()
    {
        healthSlider.value = Health;
        healthText.text = Health.ToString();
        statHelper.UpdateAllStatUI();
    }

    // ---------------- ENEMY AI ----------------

    public void Initialize()
    {
        if (deck.Cards.Count == 0)
        {
            Debug.LogError("No cards in deck");
            return;
        }

        for (int i = 0; i < 3; i++)
            DrawCard();
    }

    public void Play(float delay)
    {
        Invoke(nameof(PlayCard), delay);
    }

    private void PlayCard()
    {
        SlotManager.Instance.EnemyCards[0]
            .OnFinishedPlayingEvent
            .AddListener(() => GamePlayState.Instance.ChangeToNextState());

        SlotManager.Instance.EnemyPlayCard(
            deck.Cards[Random.Range(0, deck.Cards.Count)]
        );
    }

    private void DrawCard()
    {
        SlotManager.Instance.SpawnEnemyCard(
            deck.Cards[Random.Range(0, deck.Cards.Count)]
        );
    }
}
