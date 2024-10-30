using UnityEngine;

namespace Karin
{

    [CreateAssetMenu(menuName = "SO/CardDataSO", fileName = "Card")]
    public class CardBaseSO : ScriptableObject
    {
        public CountType count;
        public BaseShapeType Shape;
        public SpecialShapeType SpecialShape;

        public virtual void Ability()
        {

        }
    }

}