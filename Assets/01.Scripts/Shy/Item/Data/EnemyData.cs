using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase = Karin.CardBase;

namespace Shy
{
    [CreateAssetMenu(menuName = "SO/Shy/Data/Enemy")]
    public class EnemyData : Item_DataSO
    {
        public int life;
        public List<CardBase> cardDeck;
        public List<ArtifactData> artifacts;
    }
}