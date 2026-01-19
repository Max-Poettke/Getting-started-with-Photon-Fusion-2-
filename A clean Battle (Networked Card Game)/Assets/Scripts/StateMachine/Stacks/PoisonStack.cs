public class PoisonStack : ScriptableStat
{
    public override void Initialize(){
        description = "Poison\nDeals damage at the start of the turn";
    }

    public override void OnTick(EntityState owner)
    {
        // Poison damage should ignore shields
        owner.TakeDamage(amount, ignoreShield: true);
    }
}
