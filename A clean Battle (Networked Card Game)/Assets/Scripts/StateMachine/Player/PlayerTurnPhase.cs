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
        GamePlayState.Instance.PlayerStates.ForEach(x => x.ClearHand());
        GamePlayState.Instance.MainTurnCard.InitializeTurnCard("Player Turn");
        GamePlayState.Instance.MainTurnCard.gameObject.SetActive(true);
    }
}
