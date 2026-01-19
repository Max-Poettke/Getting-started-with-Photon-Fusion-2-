using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public abstract class EntityState : MonoBehaviour
{
    [Header("Health")]
    public int Health;
    public int MaxHealth;
    public bool IsAlive = true;

    [Header("Stats")]
    public List<ScriptableStat> stats = new();

    [Header("Events")]
    public UnityEvent OnTakeDamage = new();
    public UnityEvent OnDie = new();

    [Header("Visuals")]
    [SerializeField] protected Color damageColor = Color.red;
    [SerializeField] protected Color blockedColor = Color.blue;
    [SerializeField] protected EntityAnimation entityAnimation;
    [SerializeField] protected StatHelper statHelper;

    protected virtual void Awake()
    {
        statHelper = GetComponentInChildren<StatHelper>();
        entityAnimation = GetComponent<EntityAnimation>();
        InitializeUI();
    }

    // ---------------- DAMAGE PIPELINE ----------------

    public void TakeDamage(int amount, bool ignoreShield = false)
    {
        var context = new DamageContext
        {
            Amount = amount,
            IgnoreShield = ignoreShield,
            Target = this
        };

        ModifyIncomingDamage(context);
        ApplyDamage(context);
        AfterDamageApplied(context);
    }

    public void Heal(int amount){
        var context = new DamageContext
        {
            Amount = -amount,
            Target = this
        };
        
        ModifyIncomingHeal(context);
        ApplyDamage(context);
        AfterDamageApplied(context);
    }

    protected virtual void ModifyIncomingHeal(DamageContext context)
    {
        foreach (var stat in stats)
        {
            stat.OnBeforeHeal(context);
        }
    }

    protected virtual void ModifyIncomingDamage(DamageContext context)
    {
        foreach (var stat in stats)
        {
            stat.OnBeforeDamage(context);
        }
    }

    protected virtual void ApplyDamage(DamageContext context)
    {
        Health -= context.Amount;
        UpdateUI();

        if (context.Amount > 0)
        {
            entityAnimation.AnimateTakeDamage(damageColor);
            OnTakeDamage.Invoke();
        }
        else
        {
            entityAnimation.AnimateTakeDamage(blockedColor);
        }

        if (Health <= 0)
            Die();
    }

    protected virtual void AfterDamageApplied(DamageContext context)
    {
        foreach (var stat in stats)
        {
            stat.OnAfterDamage(context);
        }
    }

    protected virtual void Die()
    {
        IsAlive = false;
        OnDie.Invoke();
    }

    // ---------------- STATS ----------------

    public void AddStat(ScriptableStat stat)
    {
        stats.Add(stat);
        statHelper.AddOrUpdateStatCluster(stat);
    }

    public void RemoveStat(ScriptableStat stat)
    {
        stats.Remove(stat);
    }

    public void RemoveStatAndUpdateUI(ScriptableStat stat)
    {
        stats.Remove(stat);

        bool clusterStillExists = stats.Exists(s =>
            s.statType == stat.statType &&
            s.tickInXRounds == stat.tickInXRounds
        );

        if (!clusterStillExists)
        {
            statHelper.DestroyStatCluster(stat.statType, stat.tickInXRounds);
        }
    }

    public void Tick()
    {
        var expired = stats.FindAll(s => s.tickInXRounds == 0);

        foreach (var stat in expired)
        {
            stat.OnTick(this);
            RemoveStatAndUpdateUI(stat);
        }

        foreach (var stat in stats)
            stat.tickInXRounds--;

        statHelper.UpdateAllStatUI();
    }

    public int GetStatAmount(StatHelper.StatType type, int tickInXRounds)
    {
        return stats.FindAll(s => s.statType == type && s.tickInXRounds == tickInXRounds).Sum(s => s.amount);
    }

    // ---------------- ABSTRACT ----------------

    protected abstract void InitializeUI();
    protected abstract void UpdateUI();
}

public class DamageContext
{
    public int Amount;
    public bool IgnoreShield;
    public EntityState Source;
    public EntityState Target;
}

