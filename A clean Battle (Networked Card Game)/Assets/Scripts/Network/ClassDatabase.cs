using UnityEngine;
using System.Collections.Generic;

public class ClassDatabase : MonoBehaviour
{
    public static ClassDatabase Instance;

    public List<Sprite> classImages;
    public List<string> classNames;
    public List<string> classDescriptions;

    public int ClassCount => classImages.Count;

    private void Awake()
    {
        Instance = this;
    }
}
