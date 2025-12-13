using UnityEngine;
using Fusion;

public class NetworkedChildObject : NetworkBehaviour
{
    [Networked]
    public NetworkObject Parent {get; set;}
    [Networked]
    public string NickName {get; set;}
}
