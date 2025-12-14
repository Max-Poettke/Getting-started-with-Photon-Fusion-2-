using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;

public class DisconnectButton : MonoBehaviour
{
    [SerializeField] private NetworkRunner runner;
    [SerializeField] private string returnSceneName;
    
    private void Awake(){
        runner = FindObjectOfType<NetworkRunner>();
    }

    public void OnClick(){
        Disconnect();
    }

    private async void Disconnect(){
        if(runner != null && runner.IsRunning){
            await runner.Shutdown();
        }

        SceneManager.LoadScene(returnSceneName);
    }
}
