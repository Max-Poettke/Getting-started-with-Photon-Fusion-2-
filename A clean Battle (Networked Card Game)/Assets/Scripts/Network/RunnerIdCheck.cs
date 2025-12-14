using UnityEngine;
using Fusion;

public class RunnerIdCheck : MonoBehaviour
{
    void Awake(){
        var runner = GetComponent<NetworkRunner>();
        Debug.Log("Runner ID: " + runner.GetInstanceID());
    }
}
