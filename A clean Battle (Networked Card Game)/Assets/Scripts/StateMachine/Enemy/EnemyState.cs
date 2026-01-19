using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EnemyState : MonoBehaviour
{
    public int Health = 15;
    public int MaxHealth = 15;

    public List<ScriptableStat> stats;
    public List<PoisonStack> poisonStacks;
    public List<ShieldStack> shieldStacks;
    public List<ThreatStack> threatStacks;
    public DeckData deck;

    [Header("Animations")]
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private Color blockedColor = Color.blue;

    [Header("UI Components")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private EntityAnimation entityAnimation;

    public bool IsAlive = true;

    public UnityEvent OnTakeDamage = new UnityEvent();
    public UnityEvent OnDie = new UnityEvent();

    public StatHelper statHelper;

    private void Start(){
        statHelper = transform.GetComponentInChildren<StatHelper>();
        healthSlider.maxValue = MaxHealth;
        healthSlider.value = Health;
        healthText.text = Health.ToString();
        entityAnimation = transform.GetComponent<EntityAnimation>();
    }

    private int tickInXRounds = 1;
    private void Update(){
        //test
        if(Input.GetKeyDown(KeyCode.S)){
            //ShieldStack newShieldStack = ScriptableStat.CreateStat(-1, null, StatHelper.StatType.Shield, 1, tickInXRounds) as ShieldStack;
            //AddStat(newShieldStack);
            //tickInXRounds++;
        } else if (Input.GetKeyDown(KeyCode.D)){
            if(stats.Count == 0){
                return;
            }
            //RemoveStat(stats[0]);
        }
    }

    public void Initialize(){
        if(deck.Cards.Count == 0) {
            Debug.LogError("No cards in deck");
            return;
        }
        for(int i = 0; i < 3; i++){
            DrawCard();
        }
    }

    public void Play(float delay){
        Invoke("PlayCard", delay);
    }

    public void PlayCard(){
        SlotManager.Instance.EnemyCards[0].OnFinishedPlayingEvent.AddListener(() => GamePlayState.Instance.ChangeToNextState());
        SlotManager.Instance.EnemyPlayCard(deck.Cards[UnityEngine.Random.Range(0, deck.Cards.Count)]);
    }

    public void DrawCard(){
        if(deck.Cards.Count == 0) {
            Debug.LogError("No cards in deck");
            return;
        }
        SlotManager.Instance.SpawnEnemyCard(deck.Cards[UnityEngine.Random.Range(0, deck.Cards.Count)]);
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
                        statHelper.DestroyStat(StatHelper.StatType.Shield, 0);
                    }
                }
            }
        }
        
        Health -= _damageAmount;
        Debug.Log("Health: " + Health);
        UpdateUI();
        
        if(_damageAmount > 0){
            entityAnimation.AnimateTakeDamage(damageColor);
            OnTakeDamage?.Invoke();
        } else {
            entityAnimation.AnimateTakeDamage(blockedColor);
        }

        if(Health <= 0){
            Die();
        }
    }

    public void UpdateUI(){
        healthSlider.value = Health;
        healthText.text = Health.ToString();
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
        newPoisonStack.Initialize(-1, null, StatHelper.StatType.Poison, _amount, _tickInXRounds, statHelper.PoisonDescription);
        Debug.Log(newPoisonStack);
        Debug.Log("Adding poison stack: " + newPoisonStack.tickInXRounds + " ; " + _tickInXRounds + " ; " + newPoisonStack.statType + " ; " + newPoisonStack.amount);
        AddStat(newPoisonStack);
    }

    public void AddShieldStack(int _amount, int _tickInXRounds){
        //var newShieldStack = ScriptableStat.CreateStat(PlayerNumber, null, StatHelper.StatType.Shield, _amount, _tickInXRounds);
        
        var newShieldStack = new ShieldStack();
        newShieldStack.Initialize(-1, null, StatHelper.StatType.Shield, _amount, _tickInXRounds, statHelper.ShieldDescription);
        Debug.Log(newShieldStack);
        Debug.Log("Adding shield stack: " + newShieldStack.tickInXRounds + " ; " + _tickInXRounds + " ; " + newShieldStack.statType + " ; " + newShieldStack.amount);
        //shieldStacks.Add(newShieldStack);
        AddStat(newShieldStack);
    }

    public void AddThreatStack(int _amount, int _tickInXRounds){
        var newThreatStack = new ThreatStack();
        newThreatStack.Initialize(-1, null, StatHelper.StatType.Threat, _amount, _tickInXRounds, statHelper.ThreatDescription);
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
            stat.Tick(null);
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
