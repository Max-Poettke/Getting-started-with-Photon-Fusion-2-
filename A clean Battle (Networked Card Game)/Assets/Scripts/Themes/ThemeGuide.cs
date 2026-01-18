using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteAlways]
public class ThemeGuide : MonoBehaviour
{
    public enum Type
    {
        MainColor,
        SecondaryColor,
        Text,
        ActionColor,
        ThreatColor
    }


    [SerializeField] private float alpha = 1f;
    [SerializeField] private Theme theme;
    [SerializeField] private Type type;

    private TMP_Text tmp;
    private Image image;

    private void Start(){
        SetTheme(ThemeManager.Instance.activeTheme);
        ApplyTheme();
    }

    private void OnValidate()
    {
        ApplyTheme();
    }

    public void SetTheme(Theme newTheme)
    {
        theme = newTheme;
        ApplyTheme();
    }

    public void ApplyTheme()
    {
        if (theme == null) return;

        if (tmp == null) tmp = GetComponent<TMP_Text>();
        if (image == null) image = GetComponent<Image>();

        Color color = GetColor(type);
        color.a = alpha;

        // TMP SDF handling
        if (tmp != null)
        {
            if(tmp.fontMaterial == null) return;
            if(type == Type.Text) {
                ApplyToTMP(tmp, color);
                return;
            }
        }

        // UI Image fallback
        if (image != null)
        {
            image.color = color;
        }
    }

    private void ApplyToTMP(TMP_Text tmp, Color color)
    {
        // Force TMP to initialize its material (required in edit mode)
        tmp.ForceMeshUpdate();

        Material mat = tmp.fontMaterial;
        if (mat == null)
            return;

        tmp.color = color;
        mat.SetColor("_OutlineColor", theme.OutlineColor);
        mat.SetColor("_UnderlayColor", theme.UnderlayColor);
    }

    private Color GetColor(Type _type)
    {
        return _type switch
        {
            Type.MainColor => theme.MainColor,
            Type.SecondaryColor => theme.SecondaryColor,
            Type.Text => theme.TextColor,
            Type.ActionColor => theme.ActionColor,
            Type.ThreatColor => theme.ThreatColor,
            _ => Color.white
        };
    }
}

