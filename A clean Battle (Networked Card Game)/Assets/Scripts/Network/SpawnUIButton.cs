using UnityEngine;
using Fusion;

public class SpawnUIButton : MonoBehaviour
{
    public NetworkPrefabRef Prefab;
    public NetworkParent ParentNetworkObject;
    public NetworkRunner Runner;


    public void OnClick(){
        if(Runner == null) {
            Runner = FindObjectOfType<NetworkRunner>();
        }

        if(Runner == null || !Runner.IsRunning) return;

        Runner.Spawn(Prefab, Vector3.zero, Quaternion.identity, Runner.LocalPlayer, (runner,obj) => {
            var child = obj.GetComponent<NetworkedChildObject>();
            child.Parent = ParentNetworkObject.Object;
            child.NickName = LocalPlayerData.NickName;
            LocalPlayerData.PlayerRoomID = ParentNetworkObject.playerIndex;
        });
    }
}
