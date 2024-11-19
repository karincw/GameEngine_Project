using UnityEngine;

namespace Karin
{

    [CreateAssetMenu(menuName = "SO/CardDataSO", fileName = "Card")]
    public class CardDataSO : ScriptableObject
    {
        public CardType cardType;
        public CountType count;
        public BaseShapeType shape;
        public SpecialShapeType specialShape;

        public bool IsAttackCard => specialShape == SpecialShapeType.Sword || count == CountType.ACE || count == CountType.Two;
        public bool IsDefenceCard => specialShape == SpecialShapeType.Shield || count == CountType.Three;

        #region »ý¼ºÀÚ
        public CardDataSO(CardType cardType, CountType count, BaseShapeType shape, SpecialShapeType specialShape)
        {
            this.cardType = cardType;
            this.count = count;
            this.shape = shape;
            this.specialShape = specialShape;
        }
        public CardDataSO(CardDataSO data)
        {
            this.cardType = data.cardType;
            this.count = data.count;
            this.shape = data.shape;
            this.specialShape = data.specialShape;
        }
        private CardDataSO() { }
        #endregion
    }

}