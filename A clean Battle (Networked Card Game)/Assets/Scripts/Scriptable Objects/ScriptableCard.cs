using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableCard", menuName = "Scriptable Objects/ScriptableCard")]
public abstract class ScriptableCard : ScriptableObject
{
    public Sprite MainImage;
    public string cardName;
    public string cardDescription;
    public int Cost;

    public abstract void Resolve(CardContext context);
}

public struct CardContext
{
    public PlayerState player;
    public EnemyState enemy;
    public GamePlayState game;

    public CardContext(PlayerState player, EnemyState enemy, GamePlayState game){
        this.player = player;
        this.enemy = enemy;
        this.game = game;
    }
}