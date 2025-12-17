using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableCard", menuName = "Scriptable Objects/ScriptableCard")]
public class ScriptableCard : ScriptableObject
{
    public enum Target{
        Self,
        Enemy,
        Ally,
        All
    }
    public Sprite MainImage;
    public int Cost;
    public int Threat;
    public int shield;
    public int attack;
    public Target target;
    
}
