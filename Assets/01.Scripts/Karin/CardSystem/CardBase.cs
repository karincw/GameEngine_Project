using UnityEngine;

namespace Karin
{
    public class CardBase : MonoBehaviour
    {
        [Header("Settings")]
        public CardDataSO cardData;

        [Header("States")]
        public bool isDragging;
        public bool isHovering;

        private CardVisual _cardVisual;

        public void Initialize(CardDataSO data)
        {
            _cardVisual = GetComponent<CardVisual>();
            cardData = data;
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
        public void SetHovering(bool state) => isHovering = state;
        public void SetDragging(bool state) => isDragging = state;
    }

}
