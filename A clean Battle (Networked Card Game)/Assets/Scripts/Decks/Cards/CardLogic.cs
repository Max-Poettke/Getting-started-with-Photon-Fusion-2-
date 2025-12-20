using UnityEngine;
using System;

public class CardLogic : MonoBehaviour
{
    [SerializeField]
    private ScriptableCard cardData;

    public ScriptableCard CardData => cardData;

    public PlayerState PlayerOwner { get; private set; }
    public EnemyState EnemyOwner { get; private set; }

    public event Action<CardLogic> OnPlayed;

    public void Initialize(ScriptableCard data, PlayerState owner)
    {
        cardData = data;
        PlayerOwner = owner;
        EnemyOwner = null;
    }

    public void Play()
    {
        if (!CanPlay())
            return;

        var context = new CardContext
        {
            player = PlayerOwner,
            enemy = GamePlayState.Instance.EnemyState,
            game = GamePlayState.Instance
        };

        cardData.Resolve(context);
        OnPlayed?.Invoke(this);
    }

    protected virtual bool CanPlay()
    {
        return true;
    }
}
