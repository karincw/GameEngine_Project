using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Karin
{

    public class CardVisual : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Image _cardBackground;
        [SerializeField] private Image _mainImage;
        [SerializeField] private Image[] _subImage;
        [SerializeField] private TextMeshProUGUI[] _countText;

        [Header("Flip-Settings")]
        [SerializeField] private float _flipTime;
        private CardBase _owner;

        private bool _isFront;
        private Color alphaZero = new Color(0, 0, 0, 0);
        private RectTransform _rectTrm;

        private void Awake()
        {
            _rectTrm = transform as RectTransform;
        }

        public void Initialize(CardBase card)
        {
            _owner = card;
            _isFront = false;
            SetVisual(false);
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

        [ContextMenu("FlipTest")]
        public void Flip()
        {
            _rectTrm.DORotate(new Vector3(0, 90, 0), _flipTime / 2)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                 {
                     SetVisual(!_isFront);

                     _rectTrm.DORotate(new Vector3(0, 0, 0), _flipTime / 2).SetEase(Ease.Linear);
                     _isFront = !_isFront;
                 });
        }
        public void Flip(bool front)
        {
            _rectTrm.DORotate(new Vector3(0, 90, 0), _flipTime / 2)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    SetVisual(front);

                    _rectTrm.DORotate(new Vector3(0, 0, 0), _flipTime / 2).SetEase(Ease.Linear);
                    _isFront = front;
                });
        }
    }

}