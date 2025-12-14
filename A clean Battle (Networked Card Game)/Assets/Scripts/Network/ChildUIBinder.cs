using UnityEngine;
using Fusion;

public class ChildUIBinder : NetworkBehaviour
{
    public GameObject uiPrefab;
    private GameObject uiInstance;

    public override void Spawned(){

        var parent = GetComponent<NetworkedChildObject>().Parent;
        if(parent == null) return;

        var parentUI = parent.GetComponent<NetworkParent>().uiAnchor;
        if(parentUI == null) return;

        uiInstance = Instantiate(uiPrefab, parentUI);
        parent.GetComponent<NetworkParent>().uiObject = uiInstance;
        var setNickName = uiInstance.GetComponent<SetNickName>();
        if(setNickName == null) return;
        setNickName.SetLocalNickName(GetComponent<NetworkedChildObject>().NickName);

        var classCycler = uiInstance.GetComponent<ClassCycler>();
        if(classCycler == null) return;
        classCycler.SetInteractable(GetComponent<NetworkedChildObject>().HasInputAuthority);
    }

    public override void Despawned(NetworkRunner runner, bool hasState){
        if(uiInstance != null) Destroy(uiInstance);
    }
}
