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

    public List<ScriptableStat> stats = new List<ScriptableStat>();
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
    [SerializeField] private StatHelper statHelper;

    [Header("Prefabs")]
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private GameObject poisonPrefab;
    [SerializeField] private GameObject threatPrefab;

    private int handSize;
    private int currentHandSize;
    private int actionAmount;

    private void Start(){
        healthSlider.maxValue = MaxHealth;
        healthSlider.value = Health;
        healthText.text = Health.ToString();
        statHelper = transform.GetComponentInChildren<StatHelper>();
    }

    private int tickInXRounds = 0;
    private void Update(){
        //test
        if(Input.GetKeyDown(KeyCode.S)){
            AddShieldStack(1, tickInXRounds);
            tickInXRounds++;
        } else if (Input.GetKeyDown(KeyCode.D)){
            if(stats.Count == 0){
                return;
            }
            //RemoveStat(stats[0]);
        }  
    }

    public void UpdateHandSize(int _handSize){
        handSize = _handSize;
    }

    public void Initialize(){
        actionAmount = Actions;
        handSize = StartHandSize;
        ClearHand();
    }

    public void ClearHand(){
        Actions = actionAmount;
        SlotManager.Instance.ClearPlayerCards();
    }

    public void InvokeDrawHand(float delay = 0.4f){
        Invoke("DrawHand", delay);
    }

    private void DrawHand(){
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
        actionsText.text = "A: " + Actions.ToString();

    }

    public void Die(){
        IsAlive = false;
        OnDie?.Invoke();
    }

    public void AddStat(ScriptableStat stat){
        stats.Add(stat);
        statHelper.AddNewStat(stat);
    }

    public void RemoveStat(ScriptableStat stat){
        stats.Remove(stat);
    }

    public void AddPoisonStack(int _amount, int _tickInXRounds){
        var newPoisonStack = ScriptableStat.CreateStat(PlayerNumber, null, StatHelper.StatType.Poison, _amount, _tickInXRounds);
        //poisonStacks.Add(newPoisonStack);
        AddStat(newPoisonStack);
        UpdateUI();
    }

    public void AddShieldStack(int _amount, int _tickInXRounds){
        var newShieldStack = ScriptableStat.CreateStat(PlayerNumber, null, StatHelper.StatType.Shield, _amount, _tickInXRounds);
        Debug.Log(newShieldStack);
        Debug.Log("Adding shield stack: " + newShieldStack.tickInXRounds + " ; " + _tickInXRounds + " ; " + newShieldStack.statType + " ; " + newShieldStack.amount);
        //shieldStacks.Add(newShieldStack);
        AddStat(newShieldStack);
        UpdateUI();
    }

    public void AddThreatStack(int _amount, int _tickInXRounds){
        var newThreatStack = ScriptableStat.CreateStat(PlayerNumber, null, StatHelper.StatType.Threat, _amount, _tickInXRounds);
        //ThreatStack newThreatStack = ScriptableStat.CreateStat(PlayerNumber, null, StatHelper.StatType.Threat, _amount, _tickInXRounds) as ThreatStack;
        //threatStacks.Add(newThreatStack);
        AddStat(newThreatStack);
        UpdateUI();
    }

    public void Tick(){
        List<ScriptableStat> stacksToRemove = stats.FindAll(x => x.tickInXRounds == 0);
        bool _removedShield = false;
        bool _removedPoison = false;
        bool _removedThreat = false;
        foreach(var stat in stacksToRemove){
            if(stat.statType == StatHelper.StatType.Poison){
                _removedPoison = true;
            } else if(stat.statType == StatHelper.StatType.Shield){
                _removedShield = true;
            } else if(stat.statType == StatHelper.StatType.Threat){
                _removedThreat = true;
            }
            stats.Remove(stat);
            RemoveStat(stat);
        }

        if(_removedShield){
            statHelper.DestroyStat(StatHelper.StatType.Shield, 0);
        }

        if(_removedPoison){
            statHelper.DestroyStat(StatHelper.StatType.Poison, 0);
        }

        if(_removedThreat){
            statHelper.DestroyStat(StatHelper.StatType.Threat, 0);
        }

        foreach(var stat in stats){
            stat.tickInXRounds--;
        }

        //TickShield();
        //TickPoison();
        //TickThreat();
        UpdateUI();
    }

    public void TickShield(){
        List<ShieldStack> _stacksToRemove = shieldStacks.FindAll(x => x.tickInXRounds == 0);
        Debug.Log("Shield stacks to remove: " + _stacksToRemove.Count);
        foreach (var shieldStack in _stacksToRemove)
        {
            shieldStacks.Remove(shieldStack);
            RemoveStat(shieldStack);
        }
        statHelper.DestroyStat(StatHelper.StatType.Shield, 0);

        foreach (var shieldStack in shieldStacks)
        {
            shieldStack.tickInXRounds--;
        }
    }

    public void TickPoison(){
        Health -= GetPoisonAmount(0);
        List<PoisonStack> _stacksToRemove = poisonStacks.FindAll(x => x.tickInXRounds == 0);
        foreach (var poisonStack in _stacksToRemove)
        {
            poisonStacks.Remove(poisonStack);
            RemoveStat(poisonStack);
        }
        statHelper.DestroyStat(StatHelper.StatType.Poison, 0);
        
        foreach (var poisonStack in poisonStacks)
        {
            poisonStack.tickInXRounds--;
        }     
    }

    public void TickThreat(){
        List<ThreatStack> _stacksToRemove = threatStacks.FindAll(x => x.tickInXRounds == 0);
        foreach (var threatStack in _stacksToRemove)
        {
            threatStacks.Remove(threatStack); 
            RemoveStat(threatStack);  
        }
        statHelper.DestroyStat(StatHelper.StatType.Threat, 0);
        
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
