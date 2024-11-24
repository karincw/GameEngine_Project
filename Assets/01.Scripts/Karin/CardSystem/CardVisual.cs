using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
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
        [SerializeField] private AnimationCurve _backwardFlipEase;

        [Header("Scale-Settings")]
        [SerializeField] private float _scaleDuration = 0.2f;
        [SerializeField] private float _scaleExpandValue = 1.1f;

        [Header("Move-Settings")]
        [SerializeField] private float _moveDuration = 0.2f;
        [SerializeField] private float _movePosition;
        public bool isSelected = false;

        [Header("Shake-Settings")]
        [SerializeField] private float _swapRotationAngle;
        [SerializeField] private float _swapDuration;
        [SerializeField] private int _swapVibrato;

        [Header("Punch-Settings")]
        [SerializeField] private float _selectPunchAmount;
        [SerializeField] private float _hoverPunchAngle;
        [SerializeField] private float _hoverTransition;

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
            _owner.canDrag = false;

            _owner.PointerEnterEvent += FristFlip;
            _owner.PointerExitEvent += PointerExitHandle;
            _owner.PointerUpEvent += PointerDownHandle;
        }

        public void InitializeNoEvent(CardBase card)
        {
            _owner = card;
            _isFront = true;
            SetVisual(false);
        }
        public void SetVisual(bool isFront)
        {
            CardManager cm = CardManager.Instance;
            if (isFront)
            {
                CardDataSO data = _owner.cardData;

                _cardBackground.sprite = cm.cards[(int)data.cardType];
                _mainImage.sprite = cm.ShapeToSpriteDictionary[data.specialShape];

                Color mainColor;
                if (cm.ShapeToColorDictionary.ContainsKey(data.specialShape))
                    mainColor = cm.ShapeToColorDictionary[data.specialShape];
                else
                    mainColor = Color.white;
                _mainImage.color = mainColor;


                Sprite subSprite = cm.ShapeToSpriteDictionary[(SpecialShapeType)data.shape];
                Color subColor;

                if (cm.ShapeToColorDictionary.ContainsKey((SpecialShapeType)data.shape))
                    subColor = cm.ShapeToColorDictionary[(SpecialShapeType)data.shape];
                else
                    subColor = Color.white;

                foreach (var sb in _subImage)
                {
                    sb.sprite = subSprite;
                    sb.color = subColor;
                }

                foreach (var ct in _countText)
                {
                    ct.font = subColor == Color.black ? cm.BlackFont : subColor == Color.white ? cm.BlackFont : cm.PinkFont;
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
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                 {
                     SetVisual(!_isFront);

                     _cardTrm.DORotate(new Vector3(0, 0, 0), _flipTime / 2).SetEase(_backwardFlipEase);
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

                    _cardTrm.DORotate(new Vector3(0, 0, 0), _flipTime / 2).SetEase(_backwardFlipEase);
                    _isFront = front;
                    _owner.canDrag = true;
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

        #region Punch-Section
        /// <summary>
        /// Left : 1
        /// Right : -1
        /// </summary>
        /// <param name="dir"></param>
        public void SwapAnimation(float dir)
        {
            _cardTrm.DOComplete();
            _cardTrm.DOPunchRotation((Vector3.forward * _swapRotationAngle) * dir, _swapDuration, _swapVibrato).SetId(3);
        }
        public void PunchAnimation(float dir)
        {
            _cardTrm.DOPunchPosition(_cardTrm.up * _selectPunchAmount * dir, _scaleDuration);
            _cardTrm.DOPunchRotation(Vector3.forward * (_hoverPunchAngle * 0.5f), _hoverTransition, 20).SetId(2);
        }
        #endregion

    }

}