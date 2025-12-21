using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Enemy/EvilLint/GatherDust")]
public class GatherDust : ScriptableCard
{
    public int HealAmount;
    public override void Resolve(CardContext context)
    {
        context.enemy.Health += HealAmount;
    }
}

