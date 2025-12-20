using UnityEngine;

public class StartPhase : GamePhase
{
    public StartPhase(GamePlayState game) : base(game) { }

    public override void Enter()
    {
        Debug.Log("Start Phase entered");
        // Enemy spawns
        // Enemy displays turns
        // Players spawn
    }
}
