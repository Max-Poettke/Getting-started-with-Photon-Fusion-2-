using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    public CardVisual cardVisual;
    private ICard cardLogic;
    private Selectable selectable;
    private SlotManager slotManager;
    private RectTransform cursor;
    private Canvas canvas;
    private Vector3 mouseCardOffset;


    private bool isPlayed = false;
    public bool selected;
    public bool isDragging = false;

    //an event for each PointerEvent

    public UnityEvent OnBeginDragEvent = new UnityEvent();
    public UnityEvent OnEndDragEvent = new UnityEvent();
    public UnityEvent OnDragEvent = new UnityEvent();
    public UnityEvent OnPointerEnterEvent = new UnityEvent();
    public UnityEvent OnPointerExitEvent = new UnityEvent();
    public UnityEvent OnPointerDownEvent = new UnityEvent();
    public UnityEvent OnPointerUpEvent = new UnityEvent();


    private void Start(){
        canvas = UISingleton.Instance.canvas;
        slotManager = SlotManager.Instance;
        slotManager.AddCard(this);
        selectable = GetComponent<Selectable>();
        cardLogic = GetComponent<ICard>();
    }

    public void OnBeginDrag(PointerEventData eventData){
        if(isPlayed) return;
        isDragging = true;
        selected = !selected;
        mouseCardOffset = transform.position - GetScreenPosition();
        OnBeginDragEvent.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData){
        if(isPlayed) return;
        isDragging = false;
        if(transform.parent != null){
            PositionCard();
        }
        OnEndDragEvent.Invoke();
    }

    public void OnDrag(PointerEventData eventData){
        if(isPlayed) return;
        if(isDragging){
            Vector3 cardPos2D = GetScreenPosition() + mouseCardOffset;

            transform.position = cardPos2D;
        }
        OnDragEvent.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData){
        if(isDragging) return;
        OnPointerEnterEvent.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData){
        if(isDragging) return;
        OnPointerExitEvent.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData){
        if(isPlayed) return;
        if(isDragging) return;
        OnPointerDownEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData){
        if(isPlayed) return;
        selected = !selected;
        PositionCard();
        if(isDragging) return;
        OnPointerUpEvent.Invoke();
    }


    private Vector3 GetScreenPosition(){
        if (Mouse.current == null)
            return Vector3.zero;

        Vector2 screenPos = Mouse.current.position.ReadValue();
        /*
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas.worldCamera,
            out Vector2 localPos
        );
        return new Vector3(localPos.x, localPos.y, transform.localPosition.z);
        */
        return screenPos;
    }

    public void PositionCard(){
        transform.localPosition = Vector3.zero;
        if(selected){
            transform.localPosition += Vector3.up * 300f - Vector3.up * transform.parent.position.y;
            //transform.localRotation = new Quaternion(0, 0, Mathf.DeltaAngle(0f, transform.parent.localRotation.eulerAngles.z), 0);
        }
    }

    public void PlayCard(){
        isPlayed = true;
        selected = false;
        transform.localPosition = Vector3.zero;
        cardVisual.OnPlay();
        if(cardLogic != null){
            cardLogic.PlayCard();
        }
    }

    void OnDestroy(){
        slotManager.RemoveCard(this);
    }
}
