using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatHelper : MonoBehaviour
{
    public static StatHelper Instance;
    [SerializeField] private List<Sprite> statSprites = new List<Sprite>();
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject statContainerPrefab;
    [SerializeField] private GameObject statVisualPrefab;
    [SerializeField] private Transform UIContainer;

    private List<StatContainer> statContainers;

    private void Awake(){
        Instance = this;
    }

    public void AddNewStat(ScriptableStat stat){
        if(statContainers.Count == 0 || statContainers.Find(x => x.statName == stat.statName) == null){
            GameObject _newContainerObject = Instantiate(statContainerPrefab, UIContainer);

            StatContainer _newContainer = new StatContainer(_newContainerObject, stat);
            statContainers.Add(_newContainer);

            GameObject _newSlot = Instantiate(slotPrefab, _newContainer.slotUIParent);
            _newContainer.slots.Add(_newSlot);

            GameObject _newStatVisual = Instantiate(statVisualPrefab, _newContainer.statVisualParent);
            _newContainer.statVisuals.Add(_newStatVisual);
        } else {
            StatContainer _fittingContainer = statContainers.Find(x => x.statName == stat.statName);
            GameObject _newSlot = Instantiate(slotPrefab, _fittingContainer.slotUIParent);
            _fittingContainer.slots.Add(_newSlot);

            GameObject _newStatVisual = Instantiate(statVisualPrefab, _fittingContainer.statVisualParent);
            _fittingContainer.statVisuals.Add(_newStatVisual);
        }
    }

    public void DestroyStat(ScriptableStat stat){
        StatContainer _fittingContainer = statContainers.Find(x => x.statName == stat.statName);
        
        if(_fittingContainer == null){
            return;
        }
        
        bool _containsStat = _fittingContainer.stats.Contains(stat);
        if(_containsStat){
            GameObject _slot = _fittingContainer.slots.Find(x => x == stat);
            GameObject _statVisual = _fittingContainer.statVisuals.Find(x => x == stat);
            
            _fittingContainer.stats.Remove(stat);
            _fittingContainer.slots.Remove(_slot);
            _fittingContainer.statVisuals.Remove(_statVisual);

            Destroy(_statVisual);
            Destroy(_slot);
        }

        if(_fittingContainer.stats.Count == 0){
            Destroy(_fittingContainer.container);
            statContainers.Remove(_fittingContainer);
        }
    }
}

public class StatContainer {
    public GameObject container;
    public string statName;
    public Transform slotUIParent;
    public Transform statVisualParent;
    public List<GameObject> slots;
    public List<GameObject> statVisuals;
    public List<ScriptableStat> stats;

    public StatContainer(GameObject _container, ScriptableStat _stat){
        container = _container;
        slotUIParent = _container.transform.GetChild(0);
        statVisualParent = _container.transform.GetChild(1);
        slots = new List<GameObject>();
        statVisuals = new List<GameObject>();
        statName = _stat.statName;
        stats.Add(_stat);
    }
}
