using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Karin
{

    public class CardVisual : MonoBehaviour
    {
        [SerializeField] private Image _cardBackground;
        [SerializeField] private Image _mainImage;
        [SerializeField] private Image[] _subImage;
        [SerializeField] private TextMeshProUGUI[] _countText;

        private CardBase _owner;

        private Color alphaZero = new Color(0, 0, 0, 0);

        public void Initialize(CardBase card)
        {
            _owner = card;
        }

        public void SetVisual(bool isFront)
        {
            CardManager cm = CardManager.Instance;
            if (isFront)
            {
                CardDataSO data = _owner.cardData;

                _cardBackground.sprite = cm.cards[(int)data.cardType];
                _mainImage.sprite = cm.ShapeToSpriteDictionary[data.SpecialShape];
                _mainImage.color = cm.ShapeToColorDictionary[data.SpecialShape];

                Sprite subSprite = cm.ShapeToSpriteDictionary[(SpecialShapeType)data.Shape];
                Color subColor = cm.ShapeToColorDictionary[(SpecialShapeType)data.Shape];

                foreach (var sb in _subImage)
                {
                    sb.sprite = subSprite;
                    sb.color = subColor;
                }

                bool isBlack = subColor == Color.black;

                foreach (var ct in _countText)
                {
                    ct.font = isBlack ? cm.BlackFont : cm.PinkFont;
                    ct.text = cm.GetCountText(data.count);
                }
            }
            else
            {
                _cardBackground.sprite = cm.cards[4];
                _mainImage.color = alphaZero;

                foreach (var sb in _subImage)
                {
                    sb.color = alphaZero;
                }
                foreach (var ct in _countText)
                {
                    ct.text = string.Empty;
                }
            }
        }
    }

}