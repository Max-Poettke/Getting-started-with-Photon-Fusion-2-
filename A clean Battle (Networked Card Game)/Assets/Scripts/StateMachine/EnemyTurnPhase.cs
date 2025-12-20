using UnityEngine;

public class EnemyTurnPhase : GamePhase
{
    public EnemyTurnPhase(GamePlayState game) : base(game) { }

    public override void Enter()
    {
        Debug.Log("Enemy Turn Phase entered");
        // Conditions applied
        // Shields drop
        // Enemy plays cards
    }
}
