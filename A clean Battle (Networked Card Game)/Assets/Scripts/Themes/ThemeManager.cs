using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }
    
    public List<Theme> themes;
    public Theme activeTheme;
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            CycleTheme();
            ApplyTheme();
        } else if(Input.GetKeyDown(KeyCode.R))
        {
            ApplyTheme();
        }
    }

    public void CycleTheme()
    {
        SetTheme((themes.IndexOf(activeTheme) + 1) % themes.Count);
    }

    public void SetTheme(int index)
    {
        activeTheme = themes[index];
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
