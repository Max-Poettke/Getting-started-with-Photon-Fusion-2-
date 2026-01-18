using UnityEngine;

public class PoisonStack : ScriptableStat
{
    public override void Tick(PlayerState playerState){
        if(playerState == null){
            GamePlayState.Instance.EnemyState.TakeDamage(amount, true);
            return;
        }
        playerState.TakeDamage(amount, true);
    }
}
