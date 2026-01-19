using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EntityAnimation : MonoBehaviour
{
    [SerializeField] private Transform entityTransform;
    [SerializeField] private Image entityImage;
    [SerializeField] private float strength = 1.5f;
    [SerializeField] private float duration = 0.3f;
    Sequence entitySequence;
    Sequence imageSequence;

    public void AnimateTakeDamage(Color color = default){
        KillSequence(ref entitySequence, true);
        KillSequence(ref imageSequence, true);

        if(color != default){
            entityImage.color = color;
        }

        entitySequence = DOTween.Sequence();
        
        entitySequence.Append(entityTransform.DOShakePosition(duration, strength, 5, 50f, false, true))
                    .Join(entityTransform.DOPunchRotation(Vector3.forward * 10f, duration, 10, 1f));

        imageSequence = DOTween.Sequence();
        imageSequence.Append(entityImage.DOFade(0.5f, duration / 2f))
                    .Append(entityImage.DOFade(0f, duration / 2f));

    }
    
    private void KillSequence(ref Sequence sequence, bool complete = true)
    {
        if (sequence == null || !sequence.IsActive()) return;

        if (complete)
            sequence.Complete();
        else
            sequence.Kill();

        sequence = null;
    }
}
