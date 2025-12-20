using UnityEngine;

public class PlayerTurnPhase : GamePhase
{
    public PlayerTurnPhase(GamePlayState game) : base(game) { }

    public override void Enter()
    {
        Debug.Log("Player Turn Phase entered");
        // Conditions applied
        // Shields drop
        // Cards drawn
    }
}
