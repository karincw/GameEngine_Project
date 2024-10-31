using UnityEngine;

namespace Karin
{

    [CreateAssetMenu(menuName = "SO/CardDataSO", fileName = "Card")]
    public class CardDataSO : ScriptableObject
    {
        public CardType cardType;
        public CountType count;
        public BaseShapeType Shape;
        public SpecialShapeType SpecialShape;

        public virtual void Ability()
        {

        }
    }

}