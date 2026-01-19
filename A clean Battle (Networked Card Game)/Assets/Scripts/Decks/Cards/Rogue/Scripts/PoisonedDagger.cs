using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Cards/Rogue/PoisonedDagger")]
public class PoisonedDagger : ScriptableCard
{
    public int ActionCost;
    public int Attack;
    public int Poison;
    public int Threat;
    public override void Resolve(CardContext context)
    {
        context.enemy.TakeDamage(Attack);
        PoisonStack poisonStack = new PoisonStack() { amount = Poison, tickInXRounds = 1, statType = StatHelper.StatType.Poison };
        poisonStack.Initialize();
        context.enemy.AddStat(poisonStack);
        ThreatStack threatStack = new ThreatStack() { amount = Threat, tickInXRounds = 0, statType = StatHelper.StatType.Threat };
        threatStack.Initialize();
        context.player.AddStat(threatStack);
        context.player.UseAction(ActionCost);
    }
}

