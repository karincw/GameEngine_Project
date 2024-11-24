using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardDataSO = Karin.CardDataSO;

namespace Shy
{
    [CreateAssetMenu(menuName = "SO/Shy/Data/Enemy")]
    public class EnemyData : Item_DataSO
    {
        public int life;
        public List<CardDataSO> cardDeck;
        public List<ArtifactData> artifacts;
        public AudioClip audio;
    }
}