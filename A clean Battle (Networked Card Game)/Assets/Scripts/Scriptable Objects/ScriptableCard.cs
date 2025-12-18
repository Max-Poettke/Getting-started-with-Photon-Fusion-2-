using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableCard", menuName = "Scriptable Objects/ScriptableCard")]
public class ScriptableCard : ScriptableObject
{
    public enum PrimaryTarget{
        Self,
        Enemy,
        Ally,
        All
    }
    public enum SecondaryTarget{
        Self,
        Enemy,
        Ally,
        All,
        None
    }
    public Sprite MainImage;
    public int Cost;
    public int Threat;
    public int Shield;
    public int Attack;
    public int Health;
    public PrimaryTarget primaryTarget;
    public SecondaryTarget secondaryTarget;
    
}
