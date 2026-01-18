using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class HoverInfoUI : MonoBehaviour
{
    public static HoverInfoUI Instance;
    public GameObject InfoTextPrefab;
    public Transform InfoTextParent;

    private List<GameObject> infoTexts = new List<GameObject>();
    
    private void Awake() {
        Instance = this;
    }

    public void DisplayInfo(string description){
        GameObject infoText = Instantiate(InfoTextPrefab, InfoTextParent);
        infoText.transform.GetChild(0).GetComponent<TMP_Text>().text = description;
        infoTexts.Add(infoText);
    }

    public void ClearInfo(){
        foreach(var infoText in infoTexts){
            Destroy(infoText);
        }
        infoTexts.Clear();
    }


}
