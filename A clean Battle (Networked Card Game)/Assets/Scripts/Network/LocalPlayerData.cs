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
}
