using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialShapeType = Karin.SpecialShapeType;

namespace Shy
{
    [CreateAssetMenu(menuName = "SO/Shy/Data/Card")]
    public class StickerData : Item_DataSO
    {
        public SpecialShapeType shape;
    }
}