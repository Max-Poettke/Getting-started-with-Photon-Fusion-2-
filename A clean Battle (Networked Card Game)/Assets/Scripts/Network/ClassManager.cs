using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class ClassManager : MonoBehaviour
{
    public static ClassManager Instance;
    public UnityEvent OnCycle = new UnityEvent();

    public List<Sprite> classImages;
    public List<string> classNames;
    public List<string> classDescriptions;
    //Position in List = Player position in scene, Value in List = Class index in class list
    public int[] playerClassMap = new int[4];
    

    void Awake () {
        Instance = this;
    }

    public void CyclePlayerClass(int player){
        //increment class index and call OnCycle event
        playerClassMap[player] = (playerClassMap[player] + 1) % classImages.Count;
        OnCycle.Invoke();
    }
}
