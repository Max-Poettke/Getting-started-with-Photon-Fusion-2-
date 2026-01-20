using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class NetworkSceneSetup : NetworkBehaviour
{
    //LocalPlayerData holds data of only the local player. To get the rest of the player data, we need to syncronize between them
    
    [Networked]
    [Capacity(4)]
    public NetworkArray<PlayerData> PlayerDataArray => default;

    public override void Spawned(){
        if(Object.HasStateAuthority){
            SetPlayerData(LocalPlayerData.PlayerRoomID, LocalPlayerData.PlayerClass);
        } else {
            RPC_SetPlayerData(LocalPlayerData.PlayerRoomID, LocalPlayerData.PlayerClass);
        }
    }

    public void SetPlayerData(int playerRoomID, int playerClass){
        PlayerDataArray.Set(playerRoomID, new PlayerData(){
            nickName = LocalPlayerData.NickName,
            playerRoomID = playerRoomID,
            playerClass = playerClass
        });
        
        foreach(var player in PlayerDataArray){
            Debug.Log("PlayerDataArray: " + player.nickName + " " + player.playerRoomID + " " + player.playerClass);
        }
    }
    
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SetPlayerData(int playerRoomID, int playerClass){
        SetPlayerData(playerRoomID, playerClass);
    }
}

public struct PlayerData : INetworkStruct{
    public NetworkString<_16> nickName;
    public int playerRoomID;
    public int playerClass;
}
