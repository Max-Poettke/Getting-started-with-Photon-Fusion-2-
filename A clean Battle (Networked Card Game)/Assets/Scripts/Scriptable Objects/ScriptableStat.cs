using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableStat", menuName = "Scriptable Objects/ScriptableStat")]
public abstract class ScriptableStat : ScriptableObject
{
    public int playerNumber;
    public Sprite icon;
    public string statName;
    public int amount;
    public int tickInXRounds;

    public abstract void Tick();
}
