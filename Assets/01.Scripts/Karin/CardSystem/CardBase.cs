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
        public bool isDragging;
        public bool indexChange;
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
        private GraphicRaycaster _graphicRaycaster;
        private Image _imageCompo;
        private RectTransform _rectTrm;
        private Vector3 _offset;
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
            indexChange = false;
        }

        private void Update()
        {
            DragFollow();
        }

        public bool CanUse(int c, BaseShapeType s)
        {
            if (cardData == null) return false;

            if (cardData.count.Equals(c) || ((int)cardData.shape & (int)s) > 0)
            {
                return true;
            }

            return false;
        }
        private void DragFollow()
        {
            if (!isDragging) return;

            Vector2 targetPos = _mainCam.ScreenToWorldPoint(Input.mousePosition) - _offset;
            Vector2 delta = (targetPos - (Vector2)transform.position);

            Vector2 velocity = delta.normalized * Mathf.Min(_moveSpeedLimit, delta.magnitude / Time.deltaTime);

            transform.Translate(velocity * Time.deltaTime);
        }
        public void Swap(float dir)
        {
            _cardVisual.SwapAnimation(dir);
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
            indexChange = false;
            Vector2 mousePosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            _slibingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            _offset = mousePosition - (Vector2)transform.position;
            _graphicRaycaster.enabled = false;
            _imageCompo.raycastTarget = false;
            isDragging = true;
            _cardHolder.SelectCard = this;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            EndDragEvent?.Invoke();

            _cardHolder.SelectCard = null;
            _graphicRaycaster.enabled = true;
            _imageCompo.raycastTarget = true;
            isDragging = false;
            _cardVisual.isSelected = false;
            if (!indexChange)
                transform.SetSiblingIndex(_slibingIndex);
            else
                _cardHolder.SortingLayerOrder();
            _cardHolder.ApplyLayoutWithTween(0.3f);
            _rectTrm.DOLocalMoveY(originPos.y, 0.3f);

        }
        public void OnDrag(PointerEventData eventData)
        {

        }
        #endregion
    }

}
