using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatHelper : MonoBehaviour
{
    public enum StatType {
        Shield,
        Poison,
        Threat
    }
    [SerializeField] private List<Sprite> statSprites = new List<Sprite>();
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject statContainerPrefab;
    [SerializeField] private GameObject statVisualPrefab;
    [SerializeField] private Transform statParent;
    [SerializeField] private Transform statVisualParent;
    
    public string PoisonDescription = "POISON\nDeals damage at the start of the round";
    public string ShieldDescription = "SHIELD\nReduces damage taken";
    public string ThreatDescription = "THREAT\nThe player with the most threat at the end of the turn, will be the main target of the next attack";

    private List<StatContainer> statContainers = new List<StatContainer>();

    public void AddNewStat(ScriptableStat stat){
        if(stat.statType != null){
            Debug.Log("Stat type: " + stat.statType);
            stat.icon = statSprites[(int)stat.statType];
        }
        if(statContainers.Count == 0 || statContainers.Find(x => x.statType == stat.statType) == null){
            GameObject _newContainerObject = Instantiate(statContainerPrefab, statParent);

            StatContainer _newContainer = new StatContainer(_newContainerObject, _newContainerObject.transform, statVisualParent, stat);
            statContainers.Add(_newContainer);

            GameObject _newSlot = Instantiate(slotPrefab, _newContainer.slotUIParent);
            _newContainer.slots.Add(_newSlot);

            GameObject _newStatVisual = Instantiate(statVisualPrefab, _newContainer.statVisualParent);
            HoverInfo hoverInfo = _newStatVisual.AddComponent<HoverInfo>();
            hoverInfo.Descriptions.Add(stat.description);

            _newContainer.statVisuals.Add(_newStatVisual);
            UIFollowAnimated _newStatVisualScript = _newStatVisual.GetComponent<UIFollowAnimated>();
            _newStatVisualScript.Initialize(_newSlot.transform);
            UpdateStatUI(_newContainer, _newContainer.stats.Count - 1);
        } else {
            //Check if there is already a stat with the same tickInXRounds, if so, increase the amount instead of adding new UI
            if(statContainers.Find(x => x.statType == stat.statType && x.stats.Find(y => y.tickInXRounds == stat.tickInXRounds)) != null){
                StatContainer _fittingContainer = statContainers.Find(x => x.statType == stat.statType);
                int _statIndex = _fittingContainer.stats.FindIndex(x => x.tickInXRounds == stat.tickInXRounds);
                _fittingContainer.stats[_statIndex].amount += stat.amount;
                UpdateStatUI(_fittingContainer, _statIndex);
                Debug.Log("Stat already exists, increasing amount");
            } else {
                StatContainer _fittingContainer = statContainers.Find(x => x.statType == stat.statType);
                _fittingContainer.stats.Add(stat);
                GameObject _newSlot = Instantiate(slotPrefab, _fittingContainer.slotUIParent);
                _fittingContainer.slots.Add(_newSlot);

                GameObject _newStatVisual = Instantiate(statVisualPrefab, _fittingContainer.statVisualParent);
                HoverInfo hoverInfo = _newStatVisual.AddComponent<HoverInfo>();
                hoverInfo.Descriptions.Add(stat.description);
                
                _fittingContainer.statVisuals.Add(_newStatVisual);
                UIFollowAnimated _newStatVisualScript = _newStatVisual.GetComponent<UIFollowAnimated>();
                _newStatVisualScript.Initialize(_newSlot.transform);
                UpdateStatUI(_fittingContainer, _fittingContainer.stats.Count - 1);
            }
        }
    }

    public void DestroyStat(StatType _statType, int _tickInXRounds){
        StatContainer _fittingContainer = statContainers.Find(x => x.statType == _statType);
        //Debug.Log("remaining stats: " + _fittingContainer.stats.Count);
        
        if(_fittingContainer == null){
            Debug.Log("No container found");
            return;
        }
        
        ScriptableStat stat = _fittingContainer.stats.Find(x => x.tickInXRounds == _tickInXRounds);
        int _statIndex = _fittingContainer.stats.IndexOf(stat);
        Debug.Log("Stat index: " + _statIndex + "statType: " + _statType + "tickInXRounds: " + _tickInXRounds);
        if(_statIndex != -1){
            GameObject _slot = _fittingContainer.slots[_statIndex];
            GameObject _statVisual = _fittingContainer.statVisuals[_statIndex];
            
            _fittingContainer.stats.Remove(stat);
            _fittingContainer.slots.Remove(_slot);
            _fittingContainer.statVisuals.Remove(_statVisual);

            //Debug.Log(_statVisual.name);

            Destroy(_statVisual);
            Destroy(_slot);
        }

        if(_fittingContainer.stats.Count == 0){
            Debug.Log("Destroying container");
            Destroy(_fittingContainer.container);
            statContainers.Remove(_fittingContainer);
        }
    }

    private void UpdateStatUI(StatContainer _fittingContainer, int _statIndex){
        _fittingContainer.statVisuals[_statIndex].GetComponent<Image>().sprite = _fittingContainer.stats[_statIndex].icon;
        _fittingContainer.statVisuals[_statIndex].GetComponentInChildren<TMP_Text>().text = _fittingContainer.stats[_statIndex].amount.ToString();
    }

    public void UpdateAllStatUI(){
        foreach(var container in statContainers){
            for(int i = 0; i < container.stats.Count; i++){
                UpdateStatUI(container, i);
            }
        }
    }
}

public class StatContainer {
    public GameObject container;
    public StatHelper.StatType statType;
    public Transform slotUIParent;
    public Transform statVisualParent;
    public List<GameObject> slots;
    public List<GameObject> statVisuals;
    public List<ScriptableStat> stats;

    public StatContainer(GameObject _container, Transform _slotUIParent, Transform _statVisualParent, ScriptableStat _stat){
        container = _container;
        slotUIParent = _slotUIParent;
        statVisualParent = _statVisualParent;
        slots = new List<GameObject>(); 
        statVisuals = new List<GameObject>();
        stats = new List<ScriptableStat>();
        statType = _stat.statType;
        stats.Add(_stat);
    }
}
