using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class GamePlayState : MonoBehaviour
{
    public static GamePlayState Instance { get; private set; }

    public enum GameState
    {
        Start,
        PlayerTurn,
        EnemyTurn,
        End
    }

    public event Action OnStateChanged = () => { };

    public List<PlayerState> PlayerStates;
    public EnemyState EnemyState;

    public TurnCard MainTurnCard;
    public TurnCard WinTurnCard;
    public TurnCard LoseTurnCard;
    private GamePhase currentPhase;
    public GameState CurrentState;
    private Dictionary<GameState, GamePhase> phases;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerStates = FindObjectsOfType<PlayerState>().ToList();
        EnemyState = FindObjectOfType<EnemyState>();

        if (PlayerStates.Count == 0 || EnemyState == null)
        {
            Debug.LogError("Can't start game, either no player states or no enemy state found");
            return;
        }

        InitializePhases();
        ChangeState(GameState.Start);
    }

    private void Update()
    {
        currentPhase?.Tick();
    }

    private void InitializePhases()
    {
        phases = new Dictionary<GameState, GamePhase>
        {
            { GameState.Start, new StartPhase(this) },
            { GameState.PlayerTurn, new PlayerTurnPhase(this) },
            { GameState.EnemyTurn, new EnemyTurnPhase(this) },
            { GameState.End, new EndPhase(this) }
        };
    }

    public void ChangeState(GameState newState)
    {
        currentPhase?.Exit();
        currentPhase = phases[newState];
        CurrentState = newState;
        currentPhase.Enter();
        OnStateChanged?.Invoke();
    }

    public void ChangeToNextState(){
        ChangeState(CurrentState + 1);
    }

    public PlayerState GetPlayerWithHighestThreat()
    {
        return PlayerStates.OrderByDescending(x => x.GetThreatAmount(0)).First();
    }
}
