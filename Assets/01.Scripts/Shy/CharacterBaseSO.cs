using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase = Karin.CardBase;

namespace Shy
{
    [CreateAssetMenu(menuName = "SO/Shy/Character")]
    public class CharacterBaseSO : ScriptableObject
    {
        public string cName;
        public int life;
        public List<CardBase> cardDeck;
        public List<Artifact> artifacts;
    }
}
