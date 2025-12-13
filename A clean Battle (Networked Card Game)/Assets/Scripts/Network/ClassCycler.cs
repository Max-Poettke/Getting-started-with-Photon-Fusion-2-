using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClassCycler : MonoBehaviour
{

    
    [SerializeField] private TMP_Text playerNumber;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    private int previousClassIndex = 0;
    private int player;

    void Awake(){
        button.onClick.AddListener(() => ClassManager.Instance.CyclePlayerClass(player));
        //cant use += with unity events
        ClassManager.Instance.OnCycle.AddListener(UpdateClassImage);
        player = transform.parent.GetComponent<NetworkParent>().playerIndex;
        playerNumber.text = player.ToString();
        UpdateClassImage();
    }

    public void UpdateClassImage(){
        //update if class has changed
        Debug.Log("Player " + player + " Previous class index: " + previousClassIndex);
        Debug.Log("Player " + player + " Current class index: " + ClassManager.Instance.playerClassMap[player]);
        if(previousClassIndex == ClassManager.Instance.playerClassMap[player]) return;
        previousClassIndex = ClassManager.Instance.playerClassMap[player];

        Debug.Log("Updating class image");
        //update image, description and title
        image.sprite = ClassManager.Instance.classImages[previousClassIndex];
        title.text = ClassManager.Instance.classNames[previousClassIndex];
        description.text = ClassManager.Instance.classDescriptions[previousClassIndex];
    }
    
}
