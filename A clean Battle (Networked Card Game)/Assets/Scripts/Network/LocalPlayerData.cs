using UnityEngine;
using Random = UnityEngine.Random;

public static class LocalPlayerData
{
    private static string nickName;
    public static string NickName {
        set => nickName = value;
        get {
            if(string.IsNullOrWhiteSpace(nickName)){
                var rngPlayerNumber = Random.Range(0, 9999);
                nickName = "Player " + rngPlayerNumber.ToString();
            }
            return nickName;
        }
    }

    private static int playerRoomID;
    public static int PlayerRoomID{
        set => playerRoomID = value;
        get {
            return playerRoomID;
        }
    }

    private static int playerClass;
    public static int PlayerClass {
        set => playerClass = value;
        get {
            return playerClass;
        }
    }
}
