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
        GamePlayState.Instance.MainTurnCard.InitializeTurnCard("Battle Start!");
        GamePlayState.Instance.MainTurnCard.gameObject.SetActive(true);
        GamePlayState.Instance.PlayerStates.ForEach(x => x.Initialize());
        GamePlayState.Instance.EnemyState.Initialize();
        GamePlayState.Instance.Invoke("ChangeToNextState", 1.3f);
    }
}
