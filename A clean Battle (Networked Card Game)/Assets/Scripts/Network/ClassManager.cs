using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Fusion;
using Fusion.Sockets;

public class ClassManager : NetworkBehaviour
{
    public static ClassManager Instance;

    [Networked, Capacity(4)]
    public NetworkArray<int> PlayerClasses => default;

    public override void Spawned(){
        if(Instance == null){
            Instance = this;
        }
    }

    public void RequestCycle(int playerIndex){

        if(Object.HasStateAuthority){
            Cycle(playerIndex);
        } else {
            RPC_RequestCycle(playerIndex);
        }
    }

    private void Cycle(int playerIndex){
        PlayerClasses.Set(playerIndex, (PlayerClasses[playerIndex] + 1) % ClassDatabase.Instance.ClassCount);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestCycle(int playerIndex){
        Cycle(playerIndex);
    }
}
