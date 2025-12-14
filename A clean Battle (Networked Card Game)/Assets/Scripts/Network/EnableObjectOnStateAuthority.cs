using UnityEngine;
using Fusion;

public class EnableObjectOnStateAuthority : NetworkBehaviour
{
    public override void Spawned(){
        if(!Object.HasStateAuthority){
            gameObject.SetActive(false);
        }
    }
}
