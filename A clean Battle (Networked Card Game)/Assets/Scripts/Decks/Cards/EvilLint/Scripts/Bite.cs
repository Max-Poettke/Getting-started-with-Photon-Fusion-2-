using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Enemy/EvilLint/Bite")]
public class Bite : ScriptableCard
{
    public int DamageAmount;
    public override void Resolve(CardContext context)
    {
        GamePlayState.Instance.GetPlayerWithHighestThreat().TakeDamage(DamageAmount);
    }
}

