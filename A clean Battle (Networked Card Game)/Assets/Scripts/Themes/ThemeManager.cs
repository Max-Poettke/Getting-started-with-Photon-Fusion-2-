using UnityEngine;

[ExecuteAlways]
public class ThemeManager : MonoBehaviour
{
    [SerializeField] private Theme activeTheme;

    private void OnValidate()
    {
        ApplyTheme();
    }

    private void Awake()
    {
        ApplyTheme();
    }

    public void ApplyTheme()
    {
        if (activeTheme == null) return;

        var guides = FindObjectsOfType<ThemeGuide>(true);

        foreach (var guide in guides)
        {
            guide.SetTheme(activeTheme);
        }
    }
}
