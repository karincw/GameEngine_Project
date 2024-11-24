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

        public bool IsThis(SpecialShapeType s)
        {
            return specialShape == s;
        }
        public bool IsGiveCard()
        {
            return (specialShape >= SpecialShapeType.Give2 && specialShape <= SpecialShapeType.Give7);
        }
        public bool IsAttackCard()
        {
            return (specialShape >= SpecialShapeType.Sword2 && specialShape <= SpecialShapeType.Sword7);
        }
        public bool IsDefenceCard()
        {
            return specialShape == SpecialShapeType.Shield || specialShape == SpecialShapeType.Reflect;
        }

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