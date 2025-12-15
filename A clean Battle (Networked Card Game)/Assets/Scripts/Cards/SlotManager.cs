using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public static SlotManager Instance;
    public List<Transform> Slots;
    public List<Card> Cards;
    
    [SerializeField] private GameObject cardVisualPrefab;
    [SerializeField] private Transform cardVisualParent;

    private Card hoveredCard;
    private Card draggedCard;

    void Awake(){
        if(Instance == null){
            Instance = this;
        }
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
        cardVisual.target = card;
    }

    public void BeginDrag(Card card){
        draggedCard = card;
        Debug.Log("Dragging card: " + draggedCard.name);
    }

    public void EndDrag(Card card){
        Debug.Log("End dragging card: " + card.name);
        draggedCard = null;
    }

    public void CardPointerEnter(Card card){
        hoveredCard = card;
    }

    public void CardPointerExit(Card card){
        hoveredCard = null;
    }

    public void Update(){
        UpdateSlots();
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
