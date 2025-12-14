using UnityEngine;
using Fusion;

public class NetworkGameState : NetworkBehaviour
{
    [Networked, Capacity(4)]
    public NetworkArray<int> PlayerClasses => default;

    public override void Spawned(){
        Debug.Log("Im here!");
    }

    public void Start(){
        Debug.Log("Im still here!");
    }

    void OnDestroy(){
        Debug.Log("Im destroyed!");
    }
}
