using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Cards/Bash")]
public class Bash : ScriptableCard
{
    public int ActionCost;
    public int AttackWithShield;
    public int AttackWithoutShield;
    public int Threat;
    public override void Resolve(CardContext context)
    {
        if(context.player.GetShield(0).Sum(x => x.amount) <= 0){
            context.enemy.TakeDamage(AttackWithoutShield);
        } else {
            context.enemy.TakeDamage(AttackWithShield);
        }
        context.player.AddThreatStack(Threat, 0);
        context.player.UseAction(ActionCost);
    }
}

