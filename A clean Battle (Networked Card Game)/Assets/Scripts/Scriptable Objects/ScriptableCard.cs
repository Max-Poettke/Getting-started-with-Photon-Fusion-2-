using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableCard", menuName = "Scriptable Objects/ScriptableCard")]
public abstract class ScriptableCard : ScriptableObject
{
    public Sprite MainImage;
    public string cardName;
    public int Cost;

    public abstract void Resolve(CardContext context);
}

public struct CardContext
{
    public PlayerState player;
    public EnemyState enemy;
    public GamePlayState game;
}