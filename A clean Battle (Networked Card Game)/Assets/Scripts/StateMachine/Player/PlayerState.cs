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
            //AddPoisonStack(2, tickInXRounds);
            AddShieldStack(3, tickInXRounds);
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

    public void TakeDamage(int _damageAmount, bool _ignoreShield = false){
        if(!_ignoreShield){
            List<ScriptableStat> shieldStacks = GetShield(0);
            if(shieldStacks.Count > 0){
                int remainingDamage = _damageAmount;
                for(int i = 0; i < shieldStacks.Count; i++){
                    int amount = shieldStacks[i].amount; 
                    if(remainingDamage >= amount){
                        remainingDamage -= amount;
                        stats.Remove(shieldStacks[i]);
                    } else {
                        shieldStacks[i].amount -= remainingDamage;
                        remainingDamage = 0;
                    }
                    _damageAmount = remainingDamage;
                    if(_damageAmount == 0){
                        break;
                    } else {
                        //visually remove the shield
                        statHelper.DestroyStat(StatHelper.StatType.Shield, 0);
                    }
                }
            }
        }
        
        Health -= _damageAmount;
        Debug.Log("Health: " + Health);
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
        statHelper.UpdateAllStatUI();
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
        //var newPoisonStack = ScriptableStat.CreateStat(PlayerNumber, null, StatHelper.StatType.Poison, _amount, _tickInXRounds);
        
        var newPoisonStack = new PoisonStack();
        newPoisonStack.Initialize(PlayerNumber, null, StatHelper.StatType.Poison, _amount, _tickInXRounds);
        Debug.Log(newPoisonStack);
        Debug.Log("Adding poison stack: " + newPoisonStack.tickInXRounds + " ; " + _tickInXRounds + " ; " + newPoisonStack.statType + " ; " + newPoisonStack.amount);
        AddStat(newPoisonStack);
    }

    public void AddShieldStack(int _amount, int _tickInXRounds){
        //var newShieldStack = ScriptableStat.CreateStat(PlayerNumber, null, StatHelper.StatType.Shield, _amount, _tickInXRounds);
        
        var newShieldStack = new ShieldStack();
        newShieldStack.Initialize(PlayerNumber, null, StatHelper.StatType.Shield, _amount, _tickInXRounds);
        Debug.Log(newShieldStack);
        Debug.Log("Adding shield stack: " + newShieldStack.tickInXRounds + " ; " + _tickInXRounds + " ; " + newShieldStack.statType + " ; " + newShieldStack.amount);
        //shieldStacks.Add(newShieldStack);
        AddStat(newShieldStack);
    }

    public void AddThreatStack(int _amount, int _tickInXRounds){
        var newThreatStack = new ThreatStack();
        newThreatStack.Initialize(PlayerNumber, null, StatHelper.StatType.Threat, _amount, _tickInXRounds);
        //ThreatStack newThreatStack = ScriptableStat.CreateStat(PlayerNumber, null, StatHelper.StatType.Threat, _amount, _tickInXRounds) as ThreatStack;
        //threatStacks.Add(newThreatStack);
        AddStat(newThreatStack);
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
            stat.Tick(this);
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

        UpdateUI();
    }

    public List<ScriptableStat> GetPoison(int _tickInXRounds){
        return stats.FindAll(x => x.statType == StatHelper.StatType.Poison && x.tickInXRounds == _tickInXRounds);
    }

    public List<ScriptableStat> GetShield(int _tickInXRounds){
        return stats.FindAll(x => x.statType == StatHelper.StatType.Shield && x.tickInXRounds == _tickInXRounds);
    }

    public List<ScriptableStat> GetThreat(int _tickInXRounds){
        return stats.FindAll(x => x.statType == StatHelper.StatType.Threat && x.tickInXRounds == _tickInXRounds);
    }
}
