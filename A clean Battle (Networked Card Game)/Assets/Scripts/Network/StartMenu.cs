using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private string sceneToLoadPath;
    [SerializeField] private NetworkRunner networkRunnerPrefab;

    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField roomInput;

    private NetworkRunner runnerInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    
    public void StartShared(){
        SetPlayerData();
        StartGameAsync(GameMode.Shared, roomInput.text, sceneToLoadPath);
    }

    private void SetPlayerData(){
        if(string.IsNullOrWhiteSpace(nameInput.text)){
            LocalPlayerData.NickName = "Yahut";
        } else {
            LocalPlayerData.NickName = nameInput.text;
        }
    }

    private async void StartGameAsync(GameMode mode, string roomName, string sceneName)
    {
        runnerInstance = FindObjectOfType<NetworkRunner>();
        if(runnerInstance == null){
            runnerInstance = Instantiate(networkRunnerPrefab);
        }

        var sceneRef = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        NetworkSceneInfo info = new NetworkSceneInfo();
        info.AddSceneRef(sceneRef, LoadSceneMode.Single);

        await runnerInstance.StartGame(new StartGameArgs()
        {
            Scene = info,
            GameMode = mode,
        });

        Debug.Log("Yahooooos");

        if(runnerInstance.IsSceneAuthority){
            Debug.Log("yahoo");
            runnerInstance.LoadScene(sceneName);
        }

        Debug.Log("ooohaY");

    }
}
