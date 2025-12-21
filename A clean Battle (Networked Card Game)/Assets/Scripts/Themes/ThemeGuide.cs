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
        TextColor,
        ActionColor,
        ThreatColor,
        OutlineColor,
        UnderlayColor
    }


    [SerializeField] private Theme theme;
    [SerializeField] private Type type;

    private TMP_Text tmp;
    private Image image;

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

        Color color = GetColor();

        // TMP SDF handling
        if (tmp != null)
        {
            if(tmp.fontMaterial == null) return;
            ApplyToTMP(tmp, color);
            return;
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

        switch (type)
        {
            case Type.TextColor:
                tmp.color = color;
                break;

            case Type.OutlineColor:
                mat.SetColor("_OutlineColor", color);
                break;

            case Type.UnderlayColor:
                mat.SetColor("_UnderlayColor", color);
                break;
        }
    }

    private Color GetColor()
    {
        return type switch
        {
            Type.MainColor => theme.MainColor,
            Type.SecondaryColor => theme.SecondaryColor,
            Type.TextColor => theme.TextColor,
            Type.ActionColor => theme.ActionColor,
            Type.ThreatColor => theme.ThreatColor,
            Type.OutlineColor => theme.ActionColor,   // example mapping
            Type.UnderlayColor => theme.ThreatColor,  // example mapping
            _ => Color.white
        };
    }
}
