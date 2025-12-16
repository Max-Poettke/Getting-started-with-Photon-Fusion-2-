using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CardVisual : MonoBehaviour
{
    [Header("References")]
    public Card target;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private Transform shakeTransform;
    [SerializeField] private Transform tiltTransform;
    [SerializeField] private Transform shadowTransform;

    [Header("Movement Values")]
    [SerializeField] private float maxAngle = 10f;
    [SerializeField] private float followSpeed = 0.07f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float distanceBeforeRealign = 10f;
    private Vector3 direction;
    private float targetAngle;
    private Vector3 shakeAxis = new Vector3(0, 0, 1);
    

    [Header("Scale Params")]
    [SerializeField] private float scaleOnHover = 1.07f;
    [SerializeField] private float scaleOnSelect = 1.2f;


    

    public void OnDragEnter(){
        transform.DOScale(scaleOnSelect, 0.1f);
        shadowTransform.DOLocalMove(new Vector3(50f, -50f, 0), 0.1f, false);
        SlotManager.Instance.PushcardVisualToTop(this);
    }

    public void OnDragExit(){
        transform.DOScale(1, 0.1f);
        shadowTransform.DOLocalMove(new Vector3(5f, -5f, 0), 0.1f, false);
        shakeTransform.DOShakeRotation(0.3f, shakeAxis * 10f, 10, 90f);
    }

    public void OnHoverEnter(){
        transform.DOScale(scaleOnHover, 0.1f).SetEase(Ease.OutBack);
        shakeTransform.DOShakeRotation(0.1f, shakeAxis * 10f, 10, 50f);
    }

    public void OnHoverExit(){
        transform.DOScale(1, 0.1f).SetEase(Ease.InBack);
        shakeTransform.DOShakeRotation(0.1f, shakeAxis * 10f, 10, 50f);
    }

    public void OnSelect(){
        shakeTransform.DOShakeRotation(0.1f, shakeAxis * 10f, 10, 50f);
    }

    public void OnDeselect(){
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) return;
        LerpPosition();
        LerpRotation();
    }

    private void LerpPosition(){
        transform.position = Vector3.Lerp(transform.position, target.transform.position, followSpeed);
    }

    private void LerpRotation(){
        var difference = (target.transform.position - transform.position);
        if(difference.magnitude > distanceBeforeRealign)
        {
            direction = (target.transform.position + Vector3.up * distanceBeforeRealign - transform.position).normalized;
            targetAngle = Mathf.Clamp(Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg / 3f, -maxAngle, maxAngle);
        } else {
            direction = Vector3.zero;
            targetAngle = 0;
        }

        var currentAngle = Mathf.Atan2(transform.up.x, transform.up.y) * Mathf.Rad2Deg;
        //var angle = Mathf.Lerp(0, targetAngle, difference.magnitude);
        var newAngle = -Mathf.Lerp(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }
}
