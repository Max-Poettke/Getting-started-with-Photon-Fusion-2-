using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class TurnCard : MonoBehaviour
{
    [SerializeField] private Image mainImage;
    [SerializeField] private TMP_Text text;

    public void InitializeTurnCard(string text){
        this.text.text = text;
    }

    public void InvokeDisable(float delayBeforeAnimation){
        Invoke("BeforeDisable", delayBeforeAnimation);
        Invoke("Disable", delayBeforeAnimation + 0.3f);
    }

    //mainImage color fades in
    //mainImage y size scales up to 1
    //text color fades in
    //text scales down from 1.5 to 1 with ease out

    private void OnEnable() {
        DOTween.KillAll();
        mainImage.color = new Color(mainImage.color.r, mainImage.color.g, mainImage.color.b, 0);
        mainImage.transform.localScale = new Vector3(1, 0, 1);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        text.transform.localScale = Vector3.one * 1.5f;

        mainImage
            .DOFade(1, 0.3f);

        mainImage.transform
            .DOScaleY(1, 0.3f)
            .SetEase(Ease.OutBack);


        text
            .DOFade(1, 0.3f);

        text.transform
            .DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack);

        InvokeDisable(0.7f);
    }

    //fade all to 0 and scale to 0.7
    public void BeforeDisable(){
        DOTween.KillAll();

        mainImage
            .DOFade(0, 0.3f);

        mainImage.transform
            .DOScaleY(0.7f, 0.3f)
            .SetEase(Ease.InBack);

        text
            .DOFade(0, 0.3f);

        text.transform
            .DOScale(Vector3.one * 0.7f, 0.3f)
            .SetEase(Ease.InBack);
    }

    private void Disable(){
        gameObject.SetActive(false);
    }
}
