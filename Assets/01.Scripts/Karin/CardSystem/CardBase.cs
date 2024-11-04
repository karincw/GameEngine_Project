using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Karin
{
    public class CardBase : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Settings")]
        public CardDataSO cardData;

        [Header("States")]
        [SerializeField] private bool _isDragging;
        [SerializeField] private bool _isHovering;

        [Header("Move-Settings")]
        [SerializeField] private float _moveSpeedLimit = 50f;

        public Action PointerEnterEvent;
        public Action PointerExitEvent;
        public Action PointerDownEvent;
        public Action PointerUpEvent;
        public Action BeginDragEvent;
        public Action EndDragEvent;

        [HideInInspector] public Vector3 originPos;
        private CardHolder _cardHolder;
        private CardVisual _cardVisual;
        private Camera _mainCam;
        private Vector3 _offset;
        private GraphicRaycaster _graphicRaycaster;
        private Image _imageCompo;
        private RectTransform _rectTrm;
        private float _lastPointerDownTime;
        private float _lastPointerUpTime;
        private int _slibingIndex;

        public void Initialize(CardDataSO data, CardHolder holder, GraphicRaycaster _gr)
        {
            _cardVisual = GetComponentInChildren<CardVisual>();
            _imageCompo = GetComponent<Image>();
            _rectTrm = transform as RectTransform;
            originPos = _rectTrm.localPosition;
            _graphicRaycaster = _gr;
            cardData = data;
            _cardVisual.Initialize(this);
            _mainCam = Camera.main;
            _cardHolder = holder;
        }

        private void Update()
        {
            DragFollow();
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
        private void DragFollow()
        {
            if (!_isDragging) return;

            Vector2 targetPos = _mainCam.ScreenToWorldPoint(Input.mousePosition) - _offset;
            Vector2 delta = (targetPos - (Vector2)transform.position);

            Vector2 velocity = delta.normalized * Mathf.Min(_moveSpeedLimit, delta.magnitude / Time.deltaTime);

            transform.Translate(velocity * Time.deltaTime);
        }

        #region Interface
        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnterEvent?.Invoke();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExitEvent?.Invoke();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            //if not mouse left click, then return
            if (eventData.button != PointerEventData.InputButton.Left) return;

            PointerDownEvent?.Invoke();
            _lastPointerDownTime = Time.time;

        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            float pointDownThreshold = 0.2f;
            _lastPointerUpTime = Time.time;

            //짧게 클린한건지 알아보기 위해
            bool isTab = _lastPointerUpTime - _lastPointerDownTime < pointDownThreshold;
            if (isTab)
                PointerUpEvent?.Invoke();

        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDragEvent?.Invoke();

            Vector2 mousePosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            _slibingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            _offset = mousePosition - (Vector2)transform.position;
            _graphicRaycaster.enabled = false;
            _imageCompo.raycastTarget = false;
            _isDragging = true;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            EndDragEvent?.Invoke();
            _graphicRaycaster.enabled = true;
            _imageCompo.raycastTarget = true;
            _isDragging = false;
            _cardVisual.isSelected = false;

            _cardHolder.ApplyLayoutWithTween(0.3f);
            _rectTrm.DOLocalMoveY(originPos.y, 0.3f).OnComplete(() => { transform.SetSiblingIndex(_slibingIndex); });

        }
        public void OnDrag(PointerEventData eventData)
        {

        }
        #endregion
    }

}
