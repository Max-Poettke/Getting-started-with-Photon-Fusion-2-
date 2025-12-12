using UnityEngine;
using Fusion;

public class Health : NetworkBehaviour {
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHealth {get; set;} = 100f;

    void HealthChanged(){
        Debug.Log("Health changed to: " + NetworkedHealth);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPCDealDamage(float damage){
        Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
        NetworkedHealth -= damage;
    }
}
