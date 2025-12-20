using UnityEngine;
using UnityEngine.UI;

public class UISingleton : MonoBehaviour
{
    public static UISingleton Instance;
    public Canvas canvas;

    void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }
}
