using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GamePlayState : MonoBehaviour
{
    public static GamePlayState Instance {get; private set;}
    public List <PlayerState> PlayerStates;
    public EnemyState EnemyState;
    
    
    public void Awake(){
        Instance = this;
    }

    public void Start(){
        PlayerStates = FindObjectsOfType<PlayerState>().ToList();
        EnemyState = FindObjectOfType<EnemyState>();

        if(PlayerStates.Count == 0 || EnemyState == null){
            Debug.LogError("Can't start game, either no player states or no enemy state found");
        }
    }

    public PlayerState GetPlayerWithHighestThreat(){
        return PlayerStates.OrderByDescending(x => x.GetThreatAmount(0)).First();
    }
}
