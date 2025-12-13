using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;

public class ClassCycler : MonoBehaviour
{

    
    [SerializeField] private TMP_Text playerNumber;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Toggle readyToggle;

    private int previousClassIndex = -1;
    private int player;

    private ClassManager manager;
    private NetworkObject networkObject;

    void Awake()
    {
        var parent = transform.parent.GetComponent<NetworkParent>();
        player = parent.playerIndex;

        manager = ClassManager.Instance;

        networkObject = parent.GetComponent<NetworkObject>();
        Debug.Log($"[{name}] HasInputAuthority: {networkObject.HasStateAuthority}");
        // Disable by default until authority is known
        button.interactable = false;

        button.onClick.AddListener(OnClick);
    }

    public void SetInteractable(bool interactable){
        button.interactable = interactable;
        readyToggle.interactable = interactable;
    }

    void Update(){
        UpdateClassImage();
    }

    private void OnClick(){
        manager.RequestCycle(player);
    }

    private void UpdateClassImage(){
        if(manager == null) {
            Debug.Log("Manager is null");
            return;
        }
        int current = manager.PlayerClasses[player];
        
        if(previousClassIndex == current) return;

        previousClassIndex = current;

        image.sprite = ClassDatabase.Instance.classImages[current];
        title.text = ClassDatabase.Instance.classNames[current];
        description.text = ClassDatabase.Instance.classDescriptions[current];
    }
    
}
