using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Cards/Rogue/BackStab")]
public class BackStab : ScriptableCard
{
    public int ActionCost;
    public int Attack;
    public int AttackWhenPoisoned;
    public int Threat;
    public override void Resolve(CardContext context)
    {
        if(context.enemy.GetStatAmount(StatHelper.StatType.Poison, 0) > 0){
            context.enemy.TakeDamage(AttackWhenPoisoned);
        } else {
            context.enemy.TakeDamage(Attack);
        }
        
        ThreatStack threatStack = new ThreatStack() { amount = Threat, tickInXRounds = 0, statType = StatHelper.StatType.Threat };
        threatStack.Initialize();
        context.player.AddStat(threatStack);
        
        context.player.UseAction(ActionCost);
    }
}

