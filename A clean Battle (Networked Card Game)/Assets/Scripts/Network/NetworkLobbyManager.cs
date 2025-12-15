using UnityEngine;
using Fusion;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkLobbyManager : NetworkBehaviour
{
    public static NetworkLobbyManager Instance;
    [SerializeField] private Button startGameButton;

    private NetworkRunner runner;
    [SerializeField] private NetworkPrefabRef networkGameStatePrefab;
    
    public override void Spawned(){
        if(Instance == null){
            Instance = this;
        }
        if(Object.HasStateAuthority){
            EnableStartGame(false);
        }

        runner = FindObjectOfType<NetworkRunner>();
        if(runner == null) return;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_CheckAllReady(){
        CheckAllReady();
    }

    public void CheckAllReady(){
        EnableStartGame(false);
        if(!Object.HasStateAuthority) {
            RPC_CheckAllReady();
            return;
        }
        foreach(var player in FindObjectsOfType<NetworkParent>()){
            if(player.uiObject == null) continue;
            Debug.Log("player " + player.playerIndex + " is ready: " + player.IsReady);
            if(!player.IsReady) return;
        }

        EnableStartGame(true);
    }

    private void EnableStartGame(bool enable){
        startGameButton.interactable = enable;
    }

    public void StartGame(){
        if(!Object.HasStateAuthority) return;
        Debug.Log("Starting game!");
        EnableStartGame(false);
        RPC_StartGame();
        LoadScene();
    }

    private async void LoadScene(){
        if(runner.IsSceneAuthority){
            await runner.LoadScene("Game");
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_StartGame(){
        foreach(var playerParent in FindObjectsOfType<NetworkParent>()){            
            if(playerParent.playerIndex == LocalPlayerData.PlayerRoomID){
                LocalPlayerData.PlayerClass = playerParent.SelectedClass;
            }
        }
        Debug.Log("Player " + LocalPlayerData.PlayerRoomID + " is player " + LocalPlayerData.PlayerClass);
    }
}
