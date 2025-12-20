using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class EnemySlotManager : MonoBehaviour
{
    public static EnemySlotManager Instance;
    [Header("Hand")]
    [SerializeField] private Transform handParent;
    public List<Transform> Slots;
    public List<Card> Cards;

    [Header("Playarea")]
    public Transform playArea;
    public Transform playedCardVisualParent;
    public List<Transform> PlaySlots;
    public List<Card> PlayedCards;

    [Header("Card Visuals")]
    public List<CardVisual> CardVisuals;
    [SerializeField] private Transform cardVisualParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject SlotParentPrefab;
    [SerializeField] private GameObject CardSlotPrefab;
    [SerializeField] private GameObject CardLogicPrefab;
    [SerializeField] private GameObject cardVisualPrefab;

    void Awake(){
        if(Instance == null){
            Instance = this;
        }
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);
    }

    public void SpawnCardWithParents(ScriptableCard cardData, PlayerState playerOwner){
        var _slotParent = Instantiate(SlotParentPrefab, handParent);
        var _cardSlot = Instantiate(CardSlotPrefab, _slotParent.transform);
        //var _cardLogic = Instantiate(CardLogicPrefab, _cardSlot.transform);
        var _cardLogic = _cardSlot.transform.GetChild(0).gameObject;
        Card spawnedCard = _cardLogic.GetComponent<Card>(); 
        AddCard(spawnedCard);
        spawnedCard.Initialize(cardData, playerOwner);
    }

    public void AddCard(Card card){

        //create card visual
        CardVisual cardVisual = Instantiate(cardVisualPrefab, cardVisualParent).GetComponent<CardVisual>();
        CardVisuals.Add(cardVisual);
        card.cardVisual = cardVisual;
        cardVisual.target = card;
        
        /* place to spawn a card
        var randomCard = scriptableCards[Random.Range(0, scriptableCards.Count)];

        cardVisual.image.sprite = randomCard.MainImage;
        */
        card.OnBeginDragEvent.AddListener(() => cardVisual.OnDragEnter());
        card.OnEndDragEvent.AddListener(() => cardVisual.OnDragExit());
        card.OnPointerEnterEvent.AddListener(() => cardVisual.OnHoverEnter());
        card.OnPointerExitEvent.AddListener(() => cardVisual.OnHoverExit());
        card.OnPointerUpEvent.AddListener(() => cardVisual.OnSelect());
        StartCoroutine(ApplyLayoutNextFrame());
    }

    private IEnumerator ApplyLayoutNextFrame()
    {
        yield return null;

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            handParent as RectTransform
        );

        RebuildSlotsFromHierarchy();
    }

    public void RemoveCard(Card card){
        Cards.Remove(card);
        Slots.Remove(card.transform.parent);
        CardVisuals.Remove(card.cardVisual);
        Destroy(card.cardVisual.gameObject);
        Destroy(card.transform.parent.gameObject);
        StartCoroutine(ApplyLayoutNextFrame());
    }

    public void Update(){
        UpdateSlots();
    }

    public void UpdateSlots(){
        
        StartCoroutine(ApplyLayoutNextFrame());
    }

    public void PlayCard(Card card){
        
        StartCoroutine(ApplyLayoutNextFrame());
    }

    public void ClearPlayedCards(){
        foreach(var card in PlayedCards){
            PlayedCards.Remove(card);
            PlaySlots.Remove(card.transform.parent);
            CardVisuals.Remove(card.cardVisual);
            Destroy(card.cardVisual.gameObject);
            Destroy(card.transform.parent.gameObject);
            StartCoroutine(ApplyLayoutNextFrame());
        }
    }

    private void RebuildSlotsFromHierarchy()
    {
        Slots.Clear();
        Cards.Clear();

        for (int i = 0; i < handParent.childCount; i++)
        {
            var slotParent = handParent.GetChild(i);
            var card = slotParent.GetComponentInChildren<Card>();

            if (card == null) continue;

            Slots.Add(slotParent);
            Cards.Add(card);
        }
    }
}
