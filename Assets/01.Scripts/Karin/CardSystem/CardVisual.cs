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
        [SerializeField] private RectTransform _cardTrm;
        [SerializeField] private float _flipTime;
        [SerializeField] private AnimationCurve _forwardFlipEase;
        [SerializeField] private AnimationCurve _backwardFlipEaseCurve;

        [Header("Scale-Settings")]
        [SerializeField] private float _scaleDuration = 0.2f;
        [SerializeField] private float _scaleExpandValue = 1.1f;

        [Header("Move-Settings")]
        [SerializeField] private float _moveDuration = 0.2f;
        [SerializeField] private float _movePosition;
        public bool isSelected = false;

        private CardBase _owner;
        private bool _isFront;
        private Color alphaZero = new Color(0, 0, 0, 0);
        private RectTransform _rectTrm;

        private void Awake()
        {
            _rectTrm = transform as RectTransform;
        }
        private void OnDisable()
        {
            _owner.PointerEnterEvent -= PointerEnterHandle;
            _owner.PointerExitEvent -= PointerExitHandle;
            _owner.PointerUpEvent -= PointerDownHandle;
        }

        public void Initialize(CardBase card)
        {
            _owner = card;
            _isFront = false;
            isSelected = false;
            SetVisual(false);

            _owner.PointerEnterEvent += FristFlip;
            _owner.PointerExitEvent += PointerExitHandle;
            _owner.PointerUpEvent += PointerDownHandle;
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

        #region Flip-Section
        public void Flip()
        {
            _cardTrm.DORotate(new Vector3(0, 90, 0), _flipTime / 2)
                .SetEase(_forwardFlipEase)
                .OnComplete(() =>
                 {
                     SetVisual(!_isFront);

                     _cardTrm.DORotate(new Vector3(0, 0, 0), _flipTime / 2).SetEase(_backwardFlipEaseCurve);
                     _isFront = !_isFront;
                 });
        }
        public void Flip(bool front)
        {
            _cardTrm.DORotate(new Vector3(0, 90, 0), _flipTime / 2)
                .SetEase(_forwardFlipEase)
                .OnComplete(() =>
                {
                    SetVisual(front);

                    _cardTrm.DORotate(new Vector3(0, 0, 0), _flipTime / 2).SetEase(_backwardFlipEaseCurve);
                    _isFront = front;
                });
        }
        private void FristFlip()
        {
            _owner.PointerEnterEvent -= FristFlip;
            _owner.PointerEnterEvent += PointerEnterHandle;
            Flip(true);
        }
        #endregion

        #region Scale-Section
        public void CardScaleChange(float multiplyValue = 1.1f)
        {
            _owner.transform.DOScale(Vector3.one * multiplyValue, _scaleDuration).SetEase(Ease.Linear);
        }
        private void PointerEnterHandle()
        {
            CardScaleChange(_scaleExpandValue);
        }
        private void PointerExitHandle()
        {
            CardScaleChange(1);
        }
        #endregion

        #region Move-Section
        public void MoveCard(float offset)
        {
            _owner.transform.DOLocalMoveY(_owner.originPos.y + offset, _moveDuration).SetEase(Ease.Linear);
        }
        private void PointerDownHandle()
        {
            if (!isSelected)
                MoveCard(_movePosition);
            else
                MoveCard(0);

            isSelected = !isSelected;
        }
        #endregion

    }

}