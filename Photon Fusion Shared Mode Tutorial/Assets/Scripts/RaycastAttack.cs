using UnityEngine;
using Fusion;

public class RaycastAttack : NetworkBehaviour
{
    public float Damage = 10f;

    public PlayerMovement PlayerMovement;

    void Update(){
        if(!HasStateAuthority) return;

        Ray ray = PlayerMovement.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.origin += PlayerMovement.Camera.transform.forward;

        if(Input.GetMouseButtonDown(0)){
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 0.1f);
            if(Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 10f)){
                if(hit.transform.TryGetComponent<Health>(out Health health)){
                    health.RPCDealDamage(Damage);
                }
            }
        }
    }
}
