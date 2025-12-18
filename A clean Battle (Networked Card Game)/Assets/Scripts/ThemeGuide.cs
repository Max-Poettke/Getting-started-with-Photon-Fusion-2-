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
        ThreatColor
    }

    [SerializeField] private Theme theme;
    [SerializeField] private Type type;

    private void OnValidate()
    {
        if (theme == null) return;
        ApplyTheme();
    }

    public void SetTheme(Theme newTheme)
    {
        theme = newTheme;
        ApplyTheme();
    }

    public void ApplyTheme()
    {
        Color color = GetColor();

        if (type == Type.TextColor)
        {
            if (TryGetComponent(out TMP_Text tmp))
                tmp.color = color;
        }
        else
        {
            if (TryGetComponent(out Image image))
                image.color = color;
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
            _ => Color.white
        };
    }
}
