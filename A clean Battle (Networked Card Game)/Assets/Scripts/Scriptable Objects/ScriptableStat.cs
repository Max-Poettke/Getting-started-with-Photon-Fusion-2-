using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableStat", menuName = "Scriptable Objects/ScriptableStat")]
public class ScriptableStat : ScriptableObject
{
    public int playerNumber;
    public Sprite icon;
    public StatHelper.StatType statType;
    public int amount;
    public int tickInXRounds;

    private void Initialize(int _playerNumber, Sprite _icon, StatHelper.StatType _statType, int _amount, int _tickInXRounds){
        playerNumber = _playerNumber;
        icon = _icon;
        statType = _statType;
        amount = _amount;
        tickInXRounds = _tickInXRounds;
    }

    public static ScriptableStat CreateStat(int _playerNumber, Sprite _icon, StatHelper.StatType _statType, int _amount, int _tickInXRounds){
        ScriptableStat stat = ScriptableObject.CreateInstance(typeof(ScriptableStat)) as ScriptableStat;
        stat.Initialize(_playerNumber, _icon, _statType, _amount, _tickInXRounds);
        return stat;
    }
}
