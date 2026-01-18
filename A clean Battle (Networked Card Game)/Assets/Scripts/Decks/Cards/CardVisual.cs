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
    public Image CostImage;
    public Image ThreatImage;

    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private Transform shakeTransform;
    [SerializeField] private Transform tiltTransform;
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private Transform flipTransform;
    [SerializeField] private Transform baseVisualTransform;
    [SerializeField] private Transform backVisualTransform;

    [Header("Movement Values")]
    [SerializeField] private float maxAngle = 10f;
    [SerializeField] private float followSpeed = 0.07f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float distanceBeforeRealign = 10f;
    [SerializeField] private float flipDuration = 0.5f;

    [Header("Scale Values")]
    [SerializeField] private float scaleDefault = 1f;
    [SerializeField] private float scaleOnHover = 1.1f;
    [SerializeField] private float scaleOnSelect = 1.2f;

    public bool isFlipped = true;

    private bool isHovered;

    private Vector3 direction;
    private float targetAngle;
    private readonly Vector3 shakeAxis = new Vector3(0, 0, 1);
    private Quaternion baseShakeRotation;

    // Tweens
    private Tween scaleTween;
    private Tween shakeTween;
    private Tween shadowTween;
    private Tween flipTween;

    private void Awake()
    {
        baseShakeRotation = shakeTransform.localRotation;
    }

    private void Start()
    {
        ThemeManager.Instance.ApplyTheme();
        CardFlip();
    }

    // ==============================
    // Tween Utilities
    // ==============================

    private void KillTween(ref Tween tween, bool complete = true)
    {
        if (tween == null || !tween.IsActive()) return;

        if (complete)
            tween.Complete();
        else
            tween.Kill();

        tween = null;
    }

    private void SetScale(float value, float duration, Ease ease)
    {
        KillTween(ref scaleTween);
        scaleTween = transform
            .DOScale(value, duration)
            .SetEase(ease)
            .SetUpdate(true);
    }

    private void StartShake(float duration, float strength)
    {
        KillTween(ref shakeTween, complete: false);

        shakeTween = shakeTransform
            .DOShakeRotation(duration, shakeAxis * strength, 10, 50f)
            .SetEase(Ease.OutQuad)
            .OnKill(() => shakeTransform.localRotation = baseShakeRotation);
    }

    // ==============================
    // 🔹 PUBLIC API (RESTORED)
    // ==============================

    /// <summary>
    /// Called when a card enters play.
    /// </summary>
    public void OnPlay(float scaleOnPlay = 0.7f)
    {
        ChangeScaleOnPlay(scaleOnPlay);
        SetScale(scaleDefault, 0.1f, Ease.OutQuad);
    }

    /// <summary>
    /// Adjusts base scaling when card is played.
    /// </summary>
    public void ChangeScaleOnPlay(float multiplier)
    {
        scaleDefault *= multiplier;
        scaleOnHover *= multiplier;
        scaleOnSelect *= multiplier;
    }

    /// <summary>
    /// Called when card is selected.
    /// </summary>
    public void OnSelect()
    {
        StartShake(0.15f, 12f);
    }

    // ==============================
    // Hover / Drag
    // ==============================

    public void OnHoverEnter()
    {
        if (SlotManager.Instance.isDragging) return;

        isHovered = true;
        SlotManager.Instance.isHovered = true;
        SlotManager.Instance.PushcardVisualToTop(this);

        SetScale(scaleOnHover, 0.12f, Ease.OutBack);
        StartShake(0.1f, 10f);
    }

    public void OnHoverExit()
    {
        isHovered = false;
        SlotManager.Instance.isHovered = false;

        SetScale(scaleDefault, 0.1f, Ease.InBack);
        StartShake(0.1f, 8f);
    }

    public void OnDragEnter()
    {
        isHovered = false;
        SlotManager.Instance.isHovered = false;

        KillTween(ref shakeTween);
        KillTween(ref shadowTween);

        SetScale(scaleOnSelect, 0.1f, Ease.OutQuad);

        shadowTween = shadowTransform
            .DOLocalMove(new Vector3(50f, -50f, 0f), 0.1f)
            .SetUpdate(true);

        SlotManager.Instance.PushcardVisualToTop(this);
    }

    public void OnDragExit()
    {
        KillTween(ref shakeTween);
        KillTween(ref shadowTween);

        SetScale(scaleDefault, 0.1f, Ease.OutQuad);

        shadowTween = shadowTransform
            .DOLocalMove(new Vector3(5f, -5f, 0f), 0.1f)
            .SetUpdate(true);

        StartShake(0.2f, 10f);
    }

    // ==============================
    // Flip
    // ==============================

    public void CardFlip()
    {
        KillTween(ref flipTween, complete: false);

        flipTween = flipTransform
            .DOLocalRotate(new Vector3(0f, 90f, 0f), flipDuration / 2)
            .SetEase(Ease.InQuad)
            .OnComplete(FinishFlip);
    }

    private void FinishFlip()
    {
        //Debug.Log("finishing flip");
        baseVisualTransform.gameObject.GetComponent<CanvasGroup>().alpha = isFlipped ? 1f : 0f;
        backVisualTransform.gameObject.GetComponent<CanvasGroup>().alpha = isFlipped ? 0f : 1f;
        isFlipped = !isFlipped;

        flipTween = flipTransform
            .DOLocalRotate(new Vector3(0f, 0f, 0f), flipDuration / 2)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true)
            .OnComplete(() => {
                //Debug.Log("Flip completed");
            });
    }

    // ==============================
    // Update Motion (Idle Only)
    // ==============================

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) CardFlip();
        if((target == null || isHovered) && IsAtHomePosition() && IsAtHomeRotation()) return;
        LerpPosition();
        LerpRotation();
    }

    private bool IsAtHomePosition()
    {
        return Vector3.Distance(transform.position, target.transform.position) < 0.1f;
    }

    private bool IsAtHomeRotation()
    {
        return Quaternion.Angle(transform.rotation, target.transform.parent.rotation) < 0.1f;
    }

    private void LerpPosition()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position,
            followSpeed
        );
    }

    private void LerpRotation()
    {
        Vector3 difference = target.transform.position - transform.position;

        if (difference.magnitude > distanceBeforeRealign)
        {
            direction = (target.transform.position + Vector3.up * distanceBeforeRealign - transform.position).normalized;
            targetAngle = Mathf.Clamp(
                Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg / 3f,
                -maxAngle,
                maxAngle
            );
        }
        else
        {
            targetAngle = -Mathf.DeltaAngle(0f, target.transform.parent.rotation.eulerAngles.z);
        }

        float currentAngle = Mathf.Atan2(transform.up.x, transform.up.y) * Mathf.Rad2Deg;
        float newAngle = -Mathf.Lerp(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }
}
