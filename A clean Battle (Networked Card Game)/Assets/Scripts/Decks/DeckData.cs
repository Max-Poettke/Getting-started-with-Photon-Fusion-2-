using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "Cards/Deck")]
public class DeckData : ScriptableObject
{
    public List<ScriptableCard> cards;
}
