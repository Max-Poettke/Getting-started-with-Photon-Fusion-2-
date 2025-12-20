using UnityEngine;

public class EndPhase : GamePhase
{
    public EndPhase(GamePlayState game) : base(game) { }

    public override void Enter()
    {
        Debug.Log("End Phase entered");
        // Victory / defeat resolution
    }
}

