using UnityEngine;

[ExecuteAlways]
public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }
    
    [SerializeField] private Theme activeTheme;
    [SerializeField] private Camera camera;

    private void OnValidate()
    {
        ApplyTheme();
    }

    private void Awake()
    {
        Instance = this;
        ApplyTheme();
    }

    public void ApplyTheme()
    {
        if (activeTheme == null) return;

        camera.backgroundColor = activeTheme.MainColor;

        var guides = FindObjectsOfType<ThemeGuide>(true);

        foreach (var guide in guides)
        {
            guide.SetTheme(activeTheme);
        }
    }
}
