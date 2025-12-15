using UnityEngine;

public class AwakeChecks : MonoBehaviour
{
    void Awake(){
        
        Debug.Log("NickName: " + LocalPlayerData.NickName);
        Debug.Log("PlayerRoomID: " + LocalPlayerData.PlayerRoomID);
        Debug.Log("PlayerClass: " + LocalPlayerData.PlayerClass);
    }
}
