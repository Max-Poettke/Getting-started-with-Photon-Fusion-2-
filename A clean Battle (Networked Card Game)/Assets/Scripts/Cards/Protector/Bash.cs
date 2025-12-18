using UnityEngine;
using System;

public class Bash : MonoBehaviour, ICard
{
    public ScriptableCard card {get; set;}
    public event Action OnPlayCard;
    public PlayerState PlayerOwner {get; set;}
    public EnemyState EnemyOwner {get; set;}

    public void PlayCard(){

        if(PlayerOwner == null) {
            Debug.Log("No player assigned to card: " + gameObject.name);
            return;
        }
        GamePlayState.Instance.EnemyState.TakeDamage(card.Attack);
        OnPlayCard?.Invoke();
    }
}
