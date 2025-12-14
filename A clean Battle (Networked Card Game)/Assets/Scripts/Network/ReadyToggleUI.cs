using UnityEngine;
using UnityEngine.UI;

public class ReadyToggleUI : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private Button classSelectButton;

    private NetworkParent networkParent;

    void Awake(){
        networkParent = GetComponentInParent<NetworkParent>();

        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool value){
        networkParent.SetReady(value);
        classSelectButton.interactable = !value;
        NetworkLobbyManager.Instance.CheckAllReady();
    }
}
