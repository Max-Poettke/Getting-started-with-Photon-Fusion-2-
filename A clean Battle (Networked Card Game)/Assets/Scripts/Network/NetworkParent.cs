using UnityEngine;
using Fusion;

public class NetworkParent : NetworkBehaviour {
    public Transform uiAnchor;
    public int playerIndex;
    public GameObject uiObject;
    
    [Networked]
    public int SelectedClass {get; set;}
    [Networked]
    public bool IsReady {get; set;}

    public void SetReady(bool value){
        if(Object.HasStateAuthority){
            IsReady = value;
        }
        else {
            RPC_SetReady(value);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SetReady(bool value){
        IsReady = value;
    }
}
