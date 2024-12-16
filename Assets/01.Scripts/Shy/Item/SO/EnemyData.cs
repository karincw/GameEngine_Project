using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Karin;

namespace Shy
{
    [CreateAssetMenu(menuName = "SO/Shy/Data/Enemy")]
    public class EnemyData : Item_DataSO
    {
        public int life;
        public List<EnemyHaveCard> cardDeck;
        public List<ArtifactData> artifacts;
        public AudioClip audio;
    }

    [System.Serializable]
    public class EnemyHaveCard
    {
        public StickerData _sticker;
        public CountType _count;
        public BaseShapeType _shape;
    }
}