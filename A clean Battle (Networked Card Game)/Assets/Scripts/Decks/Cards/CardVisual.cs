using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class CardVisual : MonoBehaviour
{
    [Header("References")]
    public Card target;
    public Image image;
    public TMP_Text cardName;
    public TMP_Text cardDescription;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private Transform shakeTransform;
    [SerializeField] private Transform tiltTransform;
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private Transform flipTransform;

    [SerializeField] private Transform baseVisualTransfrom;
    [SerializeField] private Transform backVisualTransform;

    [Header("Movement Values")]
    [SerializeField] private float maxAngle = 10f;
    [SerializeField] private float followSpeed = 0.07f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float distanceBeforeRealign = 10f;
    [SerializeField] private float distanceOnHover = 10f;
    [SerializeField] private float flipDuration = 0.5f;
    private Vector3 direction;
    private float targetAngle;
    private Vector3 shakeAxis = new Vector3(0, 0, 1);
    private bool isHovered = false;
    

    [Header("Scale Params")]
    [SerializeField] private float scaleDefault = 1f;
    [SerializeField] private float scaleOnHover = 1.1f;
    [SerializeField] private float scaleOnSelect = 1.2f;
    [SerializeField] private float scaleOnFlip = 1.1f;

    public bool isFlipped = true;

    //tweens
    private Tween scaleTween;
    private Tween shakeTween;
    private Tween flipTween;
    private Tween shadowTween;
    private Tween flipRotationTween;

    private bool flipCompletedNaturally = false;

    public void CardFlip(){
        Debug.Log("Flipping card: " + target.name);
        KillScaleTween();
        KillShakeTween();
        KillFlipTween();
        flipRotationTween = flipTransform.DOLocalRotate(new Vector3(0f,90f,0f), flipDuration / 2)
            .OnComplete(() => flipCompletedNaturally = true)
            .OnKill(() => {
                if(!flipCompletedNaturally){
                    flipRotationTween.Complete();
                }
                flipCompletedNaturally = false;
            })
            .OnComplete(FinishFlip);
    }
    private void FinishFlip(){
        if(isFlipped){
            baseVisualTransfrom.gameObject.SetActive(true);
            backVisualTransform.gameObject.SetActive(false);
        } else {
            baseVisualTransfrom.gameObject.SetActive(false);
            backVisualTransform.gameObject.SetActive(true);
        }
        isFlipped = !isFlipped;
        flipRotationTween = flipTransform.DOLocalRotate(new Vector3(0f,0f,0f), flipDuration / 2).SetEase(Ease.OutQuad);
    }


    
    public void ChangeScaleOnPlay(float newValue){
        scaleOnHover = newValue * scaleOnHover;
        scaleDefault = newValue * scaleDefault;
    }

    private Quaternion baseShakeRotation;

    private void Awake()
    {
        baseShakeRotation = shakeTransform.localRotation;
    }

    private void Start()
    {
        ThemeManager.Instance.ApplyTheme();
        CardFlip();
    }

    private void KillScaleTween()
    {
        scaleTween?.Kill();
    }

    private void KillShakeTween()
    {
        shakeTween?.Kill();
        shakeTransform.localRotation = baseShakeRotation;
    }

    private void KillShadowTween()
    {
        shadowTween?.Kill();
    }

    private void KillFlipTween()
    {
        flipTween?.Kill();
        flipRotationTween?.Kill();
    }

    public void OnPlay(float scaleOnPlay = 0.7f){
        ChangeScaleOnPlay(scaleOnPlay);
        KillScaleTween();
        //KillShakeTween();

        scaleTween = transform
            .DOScale(scaleDefault, 0.1f);
    }

    public void OnDragEnter()
    {
        isHovered = false;
        SlotManager.Instance.isHovered = false;
        KillScaleTween();
        KillShakeTween();
        KillShadowTween();

        scaleTween = transform
            .DOScale(scaleOnSelect, 0.1f);

        shadowTween = shadowTransform
            .DOLocalMove(new Vector3(50f, -50f, 0), 0.1f, false);

        SlotManager.Instance.PushcardVisualToTop(this);
    }

    public void OnDragExit()
    {
        KillScaleTween();
        KillShakeTween();
        KillShadowTween();

        scaleTween = transform
            .DOScale(scaleDefault, 0.1f);

        shadowTween = shadowTransform
            .DOLocalMove(new Vector3(5f, -5f, 0), 0.1f, false);

        Shake(0.3f, 10f);
    }

    public void OnHoverEnter()
    {
        if(SlotManager.Instance.isDragging) return;
        SlotManager.Instance.UpdateCardVisualLayering();
        isHovered = true;
        SlotManager.Instance.isHovered = true;
        SlotManager.Instance.PushcardVisualToTop(this);
        KillScaleTween();
        KillShakeTween();

        scaleTween = transform
            .DOScale(scaleOnHover, 0.1f)
            .SetEase(Ease.OutBack);

        Shake(0.1f, 10f);
    }

    public void OnHoverExit()
    {
        isHovered = false;
        SlotManager.Instance.isHovered = false;
        KillScaleTween();
        KillShakeTween();

        scaleTween = transform
            .DOScale(scaleDefault, 0.1f)
            .SetEase(Ease.InBack);

        Shake(0.1f, 10f);
    }

    public void OnSelect()
    {
        KillShakeTween();
        Shake(0.1f, 10f);
    }

    private void Shake(float duration, float strength)
    {
        shakeTween = shakeTransform
            .DOShakeRotation(
                duration,
                shakeAxis * strength,
                vibrato: 10,
                randomness: 50f
            )
            .SetEase(Ease.OutQuad);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) CardFlip();
        if((target == null || isHovered) && CheckHomePosition() && CheckHomeRotation()) return;
        LerpPosition();
        LerpRotation();
    }

    private bool CheckHomePosition(){
        if(Vector3.Distance(transform.position, target.transform.position) < 0.1f) return true;
        return false;
    } 

    private bool CheckHomeRotation(){
        if(Quaternion.Angle(transform.rotation, target.transform.parent.rotation) < 0.1f) return true;
        return false;
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
            targetAngle = -Mathf.DeltaAngle(0f, target.transform.parent.rotation.eulerAngles.z);
        }

        var currentAngle = Mathf.Atan2(transform.up.x, transform.up.y) * Mathf.Rad2Deg;
        //var angle = Mathf.Lerp(0, targetAngle, difference.magnitude);
        var newAngle = -Mathf.Lerp(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }
}

