using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class PlayerState : MonoBehaviour
{
    public string NickName = "Player";
    public int PlayerNumber = 0;
    public int Health = 10;
    public int MaxHealth = 10;
    public int Threat = 0;
    public int Actions = 2;
    public int Class = 0; // 0 = Protector, 1 = Fighter, 2 = Support, 3 = Rogue
    public List<PoisonStack> poisonStacks;
    public List<ShieldStack> shieldStacks;
    public List<ThreatStack> threatStacks;

    public bool IsAlive = true;

    public event Action OnTakeDamage;
    public event Action OnDie;

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
