public abstract class ScriptableStat
{
    public string description;
    public int amount;
    public int tickInXRounds;
    public StatHelper.StatType statType;

    public virtual void Initialize() {}
    public virtual void OnBeforeHeal(DamageContext ctx) {}
    public virtual void OnBeforeDamage(DamageContext ctx) {}
    public virtual void OnAfterDamage(DamageContext ctx) {}
    public virtual void OnTick(EntityState owner) {}
}