using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerState : EntityState
{
    [Header("Player")]
    public string NickName = "Player";
    public int PlayerNumber = 0;
    public int StartHandSize = 4;
    public int Actions = 2;
    public int Threat = 0;
    public DeckData deck;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text threatText;
    [SerializeField] private TMP_Text actionsText;

    private int handSize;
    private int actionAmount;

    // ---------------- INITIALIZATION ----------------

    public void Initialize()
    {
        actionAmount = Actions;
        handSize = StartHandSize;
        ClearHand();
    }

    protected override void InitializeUI()
    {
        healthSlider.maxValue = MaxHealth;
        healthSlider.value = Health;
        healthText.text = Health.ToString();
        actionsText.text = $"A: {Actions}";
        threatText.text = Threat.ToString();
    }

    protected override void UpdateUI()
    {
        healthSlider.value = Health;
        healthText.text = Health.ToString();
        actionsText.text = $"A: {Actions}";
        threatText.text = Threat.ToString();
        statHelper.UpdateAllStatUI();
    }

    // ---------------- TURN / HAND LOGIC ----------------

    public void UpdateHandSize(int size)
    {
        handSize = size;
    }

    public void ClearHand()
    {
        Actions = actionAmount;
        SlotManager.Instance.ClearPlayerCards();
        UpdateUI();
    }

    public void InvokeDrawHand(float delay = 0.4f)
    {
        Invoke(nameof(DrawHand), delay);
    }

    private void DrawHand()
    {
        for (int i = 0; i < handSize; i++)
            DrawCard();
    }

    public void DrawCard()
    {
        if (deck.Cards.Count == 0)
        {
            Debug.LogError("No cards in deck");
            return;
        }

        SlotManager.Instance.SpawnCardWithParents(
            deck.Cards[Random.Range(0, deck.Cards.Count)],
            this
        );
    }

    // ---------------- ACTIONS ----------------

    public void UseAction(int amount)
    {
        Actions -= amount;
        UpdateUI();
    }
}
