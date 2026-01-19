using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatHelper : MonoBehaviour
{
    public enum StatType
    {
        Shield,
        Poison,
        Threat
    }

    [SerializeField] private List<Sprite> statSprites;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject statContainerPrefab;
    [SerializeField] private GameObject statVisualPrefab;
    [SerializeField] private Transform statParent;
    [SerializeField] private Transform statVisualParent;
    
    public string PoisonDescription;
    public string ShieldDescription;
    public string ThreatDescription;

    private readonly List<StatContainer> statContainers = new();

    // ---------------- ADD / UPDATE ----------------

    public void AddOrUpdateStatCluster(ScriptableStat stat)
    {
        StatContainer container = GetOrCreateContainer(stat.statType);
        StatClusterUI cluster = container.clusters
            .Find(c => c.tickInXRounds == stat.tickInXRounds);

        if (cluster != null)
        {
            cluster.totalAmount += stat.amount;
            UpdateClusterUI(cluster);
            return;
        }

        CreateNewCluster(container, stat);
    }

    private void CreateNewCluster(StatContainer container, ScriptableStat stat)
    {
        GameObject slot = Instantiate(slotPrefab, container.slotUIParent);
        GameObject visual = Instantiate(statVisualPrefab, container.statVisualParent);

        UIFollowAnimated follow = visual.GetComponent<UIFollowAnimated>();
        follow.Initialize(slot.transform);

        HoverInfo hover = visual.AddComponent<HoverInfo>();
        hover.Descriptions.Add(stat.description);

        StatClusterUI cluster = new StatClusterUI
        {
            tickInXRounds = stat.tickInXRounds,
            totalAmount = stat.amount,
            slot = slot,
            visual = visual
        };

        Image img = cluster.visual.GetComponent<Image>();
        img.sprite = statSprites[(int)stat.statType];

        container.slots.Add(slot);
        container.statVisuals.Add(visual);
        container.clusters.Add(cluster);

        UpdateClusterUI(cluster);
    }

    // ---------------- REMOVE ----------------

    public void DestroyStatCluster(StatType statType, int tickInXRounds)
    {
        StatContainer container = statContainers
            .Find(c => c.statType == statType);

        if (container == null)
            return;

        StatClusterUI cluster = container.clusters
            .Find(c => c.tickInXRounds == tickInXRounds);

        if (cluster == null)
            return;

        Destroy(cluster.visual);
        Destroy(cluster.slot);

        container.clusters.Remove(cluster);

        if (container.clusters.Count == 0)
        {
            Destroy(container.container);
            statContainers.Remove(container);
        }
    }

    // ---------------- UI ----------------

    private void UpdateClusterUI(StatClusterUI cluster)
    {
        TMP_Text txt = cluster.visual.GetComponentInChildren<TMP_Text>();
        txt.text = cluster.totalAmount.ToString();
    }

    public void UpdateAllStatUI()
    {
        foreach (var container in statContainers)
        {
            foreach (var cluster in container.clusters)
                UpdateClusterUI(cluster);
        }
    }

    // ---------------- HELPERS ----------------

    private StatContainer GetOrCreateContainer(StatType type)
    {
        StatContainer container = statContainers.Find(c => c.statType == type);
        if (container != null)
            return container;

        GameObject obj = Instantiate(statContainerPrefab, statParent);

        container = new StatContainer
        {
            container = obj,
            statType = type,
            slotUIParent = obj.transform,
            statVisualParent = statVisualParent
        };

        statContainers.Add(container);
        return container;
    }
}

public class StatClusterUI
{
    public int tickInXRounds;
    public int totalAmount;
    public GameObject slot;
    public GameObject visual;
}

public class StatContainer
{
    public GameObject container;
    public StatHelper.StatType statType;

    public Transform slotUIParent;
    public Transform statVisualParent;

    public List<GameObject> slots = new();
    public List<GameObject> statVisuals = new();
    public List<StatClusterUI> clusters = new();
}


