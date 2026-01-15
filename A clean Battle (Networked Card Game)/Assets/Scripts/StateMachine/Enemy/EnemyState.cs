using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class EnemyState : MonoBehaviour
{
    public int Health = 10;
    public int MaxHealth = 10;

    public List<ScriptableStat> stats;
    public List<PoisonStack> poisonStacks;
    public List<ShieldStack> shieldStacks;
    public List<ThreatStack> threatStacks;
    public DeckData deck;

    public bool IsAlive = true;

    public event Action OnTakeDamage;
    public event Action OnDie;

    public StatHelper statHelper;

    private void Start(){
        statHelper = transform.GetComponentInChildren<StatHelper>();
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

    public void TakeDamage(int _damageAmount){
        Health -= _damageAmount;
        OnTakeDamage?.Invoke();
        if(Health <= 0){
            Die();
        }
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
        poisonStacks.Add(new PoisonStack(){amount = _amount, tickInXRounds = _tickInXRounds});
    }

    public void AddShieldStack(int _amount, int _tickInXRounds){
        shieldStacks.Add(new ShieldStack(){amount = _amount, tickInXRounds = _tickInXRounds});
    }

    public void AddThreatStack(int _amount, int _tickInXRounds){
        threatStacks.Add(new ThreatStack(){amount = _amount, tickInXRounds = _tickInXRounds});
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
        return poisonStacks.Where(x => x.tickInXRounds == _tickInXRounds).Sum(x => x.amount);
    }

    public int GetShieldAmount(int _tickInXRounds){
        return shieldStacks.Where(x => x.tickInXRounds == _tickInXRounds).Sum(x => x.amount);
    }

    public int GetThreatAmount(int _tickInXRounds){
        return threatStacks.Where(x => x.tickInXRounds == _tickInXRounds).Sum(x => x.amount);
    }
}
