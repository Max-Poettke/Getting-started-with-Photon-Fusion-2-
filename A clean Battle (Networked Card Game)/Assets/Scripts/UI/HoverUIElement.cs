using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class HoverUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleOnHover = 1.3f;
    [SerializeField] private float scaleDuration = 0.2f;
    [SerializeField] private Color colorOnHover = Color.white;
    private Color colorOnExit;

    private Tween scaleTween;
    private Tween colorTween;
    private Image image;

    public void Initialize(float _scaleOnHover, float _scaleDuration, Color _colorOnHover, Color _colorOnExit){
        scaleOnHover = _scaleOnHover;
        scaleDuration = _scaleDuration;
        colorOnHover = _colorOnHover;
        image.color = _colorOnExit;
    }

    private void Start()
    {
        image = GetComponent<Image>();
        colorOnExit = image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        colorOnExit = image.color;
        KillTween(ref scaleTween, true);
        KillTween(ref colorTween, true);
        scaleTween = transform
            .DOScale(scaleOnHover, scaleDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);
        colorTween = image.DOColor(colorOnHover, scaleDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        KillTween(ref scaleTween, true);
        KillTween(ref colorTween, true);
        scaleTween = transform
            .DOScale(1f, scaleDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);
        colorTween = image.DOColor(colorOnExit, scaleDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);
    }

    private void KillTween(ref Tween tween, bool complete = true)
    {
        if (tween == null || !tween.IsActive()) return;

        if (complete)
            tween.Complete();
        else
            tween.Kill();

        tween = null;
    }
}
