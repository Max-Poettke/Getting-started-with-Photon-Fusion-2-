using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Bash")]
public class Bash : ScriptableCard
{
    public int AttackWithShield;
    public int AttackWithoutShield;
    public int Threat;
    public override void Resolve(CardContext context)
    {
        if(context.player.GetShieldAmount(0) <= 0){
            context.enemy.TakeDamage(AttackWithoutShield);
        } else {
            context.enemy.TakeDamage(AttackWithShield);
        }
        context.player.AddThreatStack(Threat, 0);
    }
}

