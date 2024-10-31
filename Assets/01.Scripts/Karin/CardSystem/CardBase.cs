using UnityEngine;

namespace Karin
{

    public class CardBase : MonoBehaviour
    {
        public CardDataSO cardData;

        private CardVisual _cardVisual;

        private void Awake()
        {
            _cardVisual = GetComponentInChildren<CardVisual>();
        }

        private void Start()
        {
            _cardVisual.Initialize(this);
        }

        public bool CanUse(int c, BaseShapeType s)
        {
            if (cardData == null) return false;

            if (cardData.count.Equals(c) || ((int)cardData.Shape & (int)s) > 0)
            {
                return true;
            }

            return false;
        }
    }

}
