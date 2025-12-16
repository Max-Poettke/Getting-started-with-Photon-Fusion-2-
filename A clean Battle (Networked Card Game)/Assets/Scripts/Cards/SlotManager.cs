using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlotManager : MonoBehaviour
{
    public static SlotManager Instance;
    public List<Transform> Slots;
    public List<Card> Cards;
    public List<CardVisual> CardVisuals;
    
    [SerializeField] private GameObject cardVisualPrefab;
    [SerializeField] private Transform cardVisualParent;

    private Card hoveredCard;
    private Card draggedCard;

    public bool isDragging = false;

    void Awake(){
        if(Instance == null){
            Instance = this;
        }
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);
    }

    public void AddCard(Card card){
        Cards.Add(card);
        Slots.Add(card.transform.parent);
        card.OnBeginDragEvent.AddListener(() => BeginDrag(card));
        card.OnEndDragEvent.AddListener(() => EndDrag(card));
        card.OnPointerEnterEvent.AddListener(() => CardPointerEnter(card));
        card.OnPointerExitEvent.AddListener(() => CardPointerExit(card));

        //create card visual
        CardVisual cardVisual = Instantiate(cardVisualPrefab, cardVisualParent).GetComponent<CardVisual>();
        CardVisuals.Add(cardVisual);
        card.cardVisual = cardVisual;
        cardVisual.target = card;
        card.OnBeginDragEvent.AddListener(() => cardVisual.OnDragEnter());
        card.OnEndDragEvent.AddListener(() => cardVisual.OnDragExit());
        card.OnPointerEnterEvent.AddListener(() => cardVisual.OnHoverEnter());
        card.OnPointerExitEvent.AddListener(() => cardVisual.OnHoverExit());
        card.OnPointerUpEvent.AddListener(() => cardVisual.OnSelect());
    }

    public void RemoveCard(Card card){
        Cards.Remove(card);
        Slots.Remove(card.transform.parent);
        CardVisuals.Remove(card.cardVisual);
        Destroy(card.cardVisual.gameObject);
        Destroy(card.transform.parent.gameObject);
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
    }

    public void CardPointerEnter(Card card){
        hoveredCard = card;
    }

    public void CardPointerExit(Card card){
        hoveredCard = null;
    }

    public void Update(){
        UpdateSlots();
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
        if(isDragging) return;
        CardVisuals.Sort((a, b) => a.target.transform.parent.position.x.CompareTo(b.target.transform.parent.position.x));
        for(int i = 0; i < CardVisuals.Count; i++){
            CardVisuals[i].transform.SetSiblingIndex(i);
        }
    }

    public void UpdateSlots(){
        if(draggedCard == null) return;
        var draggedCardParent = draggedCard.transform.parent;

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
    }

    private void Swap(int index){
        if(draggedCard == null) return;
        Debug.Log("Swapping cards: " + draggedCard.name + " and " + Cards[index].name);
        var draggedParent = draggedCard.transform.parent;
        var distanceBetweenParents = Cards[index].transform.parent.position.x - draggedParent.position.x;
        draggedCard.transform.parent = Cards[index].transform.parent;
        draggedCard.transform.localPosition -= new Vector3(distanceBetweenParents, 0, 0);
        Cards[index].transform.parent = draggedParent;
        Cards[index].PositionCard();
        draggedCard.PositionCard();
    }
}
