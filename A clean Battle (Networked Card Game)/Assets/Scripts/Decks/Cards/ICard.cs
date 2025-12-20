using UnityEngine;
using System;

public interface ICard
{
    public ScriptableCard card {get; set;}
    public event Action OnPlayCard;
    public PlayerState PlayerOwner {get; set;}
    public EnemyState EnemyOwner {get; set;}

    public void PlayCard();
}
