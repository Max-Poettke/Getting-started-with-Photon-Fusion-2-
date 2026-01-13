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
        SlotManager.Instance.ClearPlayedCards();
        GamePlayState.Instance.MainTurnCard.InitializeTurnCard("Player Turn");
        GamePlayState.Instance.MainTurnCard.gameObject.SetActive(true);
        GamePlayState.Instance.PlayerStates.ForEach(x => x.Tick());
    }

    public override void Exit()
    {
        Debug.Log("Player Turn Phase exited");
        GamePlayState.Instance.PlayerStates.ForEach(x => {x.ClearHand();});
    }
}
