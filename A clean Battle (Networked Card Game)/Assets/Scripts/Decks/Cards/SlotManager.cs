using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public static SlotManager Instance;
    [Header("Hand")]
    [SerializeField] private Transform handParent;
    public AnimationCurve VerticalOffsetCurve;
    public AnimationCurve AngleCurve;
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

    private Card hoveredCard;
    private Card draggedCard;
    private Card playEnabledCard;
    private Transform playEnabledParentParent;

    public bool isHovered = false;
    public bool isDragging = false;

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
        card.OnBeginDragEvent.AddListener(() => BeginDrag(card));
        card.OnEndDragEvent.AddListener(() => EndDrag(card));
        card.OnPointerEnterEvent.AddListener(() => CardPointerEnter(card));
        card.OnPointerExitEvent.AddListener(() => CardPointerExit(card));

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
        SetSlotPositionAndRotation();
    }

    public void RemoveCard(Card card){
        Cards.Remove(card);
        Slots.Remove(card.transform.parent);
        CardVisuals.Remove(card.cardVisual);
        Destroy(card.cardVisual.gameObject);
        Destroy(card.transform.parent.gameObject);
        StartCoroutine(ApplyLayoutNextFrame());
    }

    public void BeginDrag(Card card){
        draggedCard = card;
        isDragging = true;
        Debug.Log("Dragging card: " + draggedCard.name);
    }

    public void EndDrag(Card card){
        Debug.Log("End dragging card: " + card.name);
        draggedCard = null;
        isDragging = false;
        TryPlayCard(card);
    }

    public void CardPointerEnter(Card card){
        hoveredCard = card;
    }

    public void CardPointerExit(Card card){
        hoveredCard = null;
    }

    public void Update(){
        UpdateSlots();
        if(isDragging || isHovered) return;
        UpdateCardVisualLayering();
    }
    
    public void PushcardVisualToTop(CardVisual cardVisual){
        CardVisuals.Remove(cardVisual);
        CardVisuals.Add(cardVisual);

        for(int i = 0; i < CardVisuals.Count; i++){
            CardVisuals[i].transform.SetSiblingIndex(i);
        }
    }

    public void UpdateCardVisualLayering(){
        CardVisuals.Sort((a, b) => a.target.transform.parent.position.x.CompareTo(b.target.transform.parent.position.x));
        for(int i = 0; i < CardVisuals.Count; i++){
            CardVisuals[i].transform.SetSiblingIndex(i);
        }
    }

    public void UpdateSlots(){
        if(draggedCard == null) return;
        var draggedCardParent = draggedCard.transform.parent;

        if(draggedCard.transform.position.y > 300){
            EnablePlayCard(draggedCard);
            return;
        }

        DisablePlayCard();

        for(int i = 0; i < Cards.Count; i++){
            if(draggedCardParent.position.x < Cards[i].transform.parent.position.x){
                if(draggedCard.transform.position.x > Cards[i].transform.position.x){
                    Swap(i);
                }
            } else if(draggedCardParent.position.x > Cards[i].transform.parent.position.x){
                if(draggedCard.transform.position.x < Cards[i].transform.position.x){
                    Swap(i);
                }
            }
        }
        StartCoroutine(ApplyLayoutNextFrame());
    }

    public void EnablePlayCard(Card card){
        playEnabledCard = card;
        playEnabledParentParent = card.transform.parent.parent;
    }

    public void DisablePlayCard(){
        playEnabledCard = null;
        playEnabledParentParent = null;
    }

    public void TryPlayCard(Card card){
        if(GamePlayState.Instance.CurrentState != GamePlayState.GameState.PlayerTurn) return;
        if(playEnabledCard != card) return;
        if(card.CardData.Cost > playEnabledCard.PlayerOwner.Actions){
            return;
        }
        var slotParent = Instantiate(SlotParentPrefab, playArea);
        card.transform.parent.parent = slotParent.transform;
        card.transform.parent.localPosition = Vector3.zero;
        card.transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
        card.transform.parent.localScale = Vector3.one;
        Slots.Remove(card.transform.parent);
        Cards.Remove(card);
        PlayedCards.Add(card);
        PlaySlots.Add(card.transform.parent);
        card.cardVisual.transform.SetParent(playedCardVisualParent);
        Destroy(playEnabledParentParent.gameObject);

        card.PlayCard();
        DisablePlayCard();
        StartCoroutine(ApplyLayoutNextFrame());
    }

    public void SetSlotPositionAndRotation()
    {
        int count = Slots.Count;
        if (count == 0) return;

        for (int i = 0; i < count; i++)
        {
            float t = (count == 1) ? 0.5f : (float)i / (count - 1);

            Slots[i].localPosition = new Vector3(
                Slots[i].localPosition.x, // owned by layout group
                VerticalOffsetCurve.Evaluate(t),
                Slots[i].localPosition.z
            );

            Slots[i].localRotation = Quaternion.Euler(
                0,
                0,
                -AngleCurve.Evaluate(t)
            );
        }
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


    private void Swap(int index){
        if(draggedCard == null) return;
        Debug.Log("Swapping cards: " + draggedCard.name + " and " + Cards[index].name);
        var draggedParent = draggedCard.transform.parent;
        var distanceBetweenParents = Cards[index].transform.parent.position.x - draggedParent.position.x;
        draggedCard.transform.parent = Cards[index].transform.parent;
        draggedCard.transform.localPosition -= new Vector3(distanceBetweenParents, 0, 0);
        Cards[index].transform.parent = draggedParent;
        Cards[index].transform.localRotation = Quaternion.Euler(0, 0, 0);
        draggedCard.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Cards[index].PositionCard();
        draggedCard.PositionCard();
    }
}
