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
        SlotManager.Instance.ClearPlayedCards();
        GamePlayState.Instance.MainTurnCard.InitializeTurnCard("Enemy Turn");
        GamePlayState.Instance.MainTurnCard.gameObject.SetActive(true);
        GamePlayState.Instance.MainTurnCard.InvokeDisable(1);
        GamePlayState.Instance.EnemyState.Play(1.2f);
        
    }
}
