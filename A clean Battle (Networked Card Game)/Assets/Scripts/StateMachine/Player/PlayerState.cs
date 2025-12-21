using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;

public class PlayerState : MonoBehaviour
{
    public string NickName = "Player";
    public int PlayerNumber = 0;
    public int StartHandSize = 4;
    public int Health = 10;
    public int MaxHealth = 10;
    public int Threat = 0;
    public int Actions = 2;
    public int Class = 0; // 0 = Protector, 1 = Fighter, 2 = Support, 3 = Rogue
    public DeckData deck;
    public List<PoisonStack> poisonStacks = new List<PoisonStack>();
    public List<ShieldStack> shieldStacks = new List<ShieldStack>();
    public List<ThreatStack> threatStacks = new List<ThreatStack>();

    public bool IsAlive = true;

    public event Action OnTakeDamage;
    public event Action OnDie;

    [Header("UI Components")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text threatText;
    [SerializeField] private TMP_Text actionsText;
    [SerializeField] private Transform shieldPosition;
    [SerializeField] private Transform poisonPosition;
    [SerializeField] private Transform threatPosition;

    [Header("Prefabs")]
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private GameObject poisonPrefab;
    [SerializeField] private GameObject threatPrefab;

    private int handSize;

    private void Start(){
        healthSlider.maxValue = MaxHealth;
        healthSlider.value = Health;
        healthText.text = Health.ToString();
    }

    public void Initialize(){
        handSize = StartHandSize;
        for(int i = 0; i < handSize; i++){
            DrawCard();
        }
    }

    public void DrawCard(){
        if(deck.Cards.Count == 0) {
            Debug.LogError("No cards in deck");
            return;
        }
        SlotManager.Instance.SpawnCardWithParents(deck.Cards[UnityEngine.Random.Range(0, deck.Cards.Count)], this);
    }

    public void UseAction(int amount){
        Actions -= amount;
        UpdateUI();
    }

    public void TakeDamage(int _damageAmount){
        Health -= _damageAmount;
        UpdateUI();
        OnTakeDamage?.Invoke();
        if(Health <= 0){
            Die();
        }
    }

    private void UpdateUI(){
        healthSlider.value = Health;
        healthText.text = Health.ToString();
        threatText.text = "T: " + GetThreatAmount(0);
        actionsText.text = "A: " + Actions.ToString();
    }

    public void Die(){
        IsAlive = false;
        OnDie?.Invoke();
    }

    public void AddPoisonStack(int _amount, int _tickInXRounds){
        poisonStacks.Add(new PoisonStack(){amount = _amount, tickInXRounds = _tickInXRounds});
        UpdateUI();
    }

    public void AddShieldStack(int _amount, int _tickInXRounds){
        shieldStacks.Add(new ShieldStack(){amount = _amount, tickInXRounds = _tickInXRounds});
        UpdateUI();
    }

    public void AddThreatStack(int _amount, int _tickInXRounds){
        threatStacks.Add(new ThreatStack(){amount = _amount, tickInXRounds = _tickInXRounds});
        UpdateUI();
    }

    public void Tick(){
        TickShield();
        TickPoison();
        TickThreat();
    }

    public void TickShield(){
        shieldStacks.RemoveAll(x => x.tickInXRounds == 0);
        foreach (var shieldStack in shieldStacks)
        {
            shieldStack.tickInXRounds--;
        }     
    }

    public void TickPoison(){
        Health -= GetPoisonAmount(0);
        poisonStacks.RemoveAll(x => x.tickInXRounds == 0);
        foreach (var poisonStack in poisonStacks)
        {
            poisonStack.tickInXRounds--;
        }     
    }

    public void TickThreat(){
        threatStacks.RemoveAll(x => x.tickInXRounds == 0);
        foreach (var threatStack in threatStacks)
        {
            threatStack.tickInXRounds--;
        }     
    }

    public int GetPoisonAmount(int _tickInXRounds){
        if(poisonStacks == null) return 0;
        if(poisonStacks.Count == 0) return 0;
        return poisonStacks.Where(x => x.tickInXRounds == _tickInXRounds).Sum(x => x.amount);
    }

    public int GetShieldAmount(int _tickInXRounds){
        if(shieldStacks == null) return 0;
        if(shieldStacks.Count == 0) return 0;
        return shieldStacks.Where(x => x.tickInXRounds == _tickInXRounds).Sum(x => x.amount);
    }

    public int GetThreatAmount(int _tickInXRounds){
        if(threatStacks == null) return 0;
        if(threatStacks.Count == 0) return 0;
        return threatStacks.Where(x => x.tickInXRounds == _tickInXRounds).Sum(x => x.amount);
    }
}
