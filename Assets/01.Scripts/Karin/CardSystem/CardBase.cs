using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Explain = Shy.ExplainManager;

namespace Karin
{
    public class CardBase : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Settings")]
        public CardDataSO cardData;
        [HideInInspector] public CardVisual CardVisual;
        [HideInInspector] public ChangeSprite ChangeSprite;
        private CardPlace _place;
        [SerializeField] private AudioClip _useCardClip;

        [Header("States")]
        public bool isDragging;
        public bool indexChange;
        public bool canDrag;
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
            CardVisual = GetComponentInChildren<CardVisual>();
            _imageCompo = GetComponent<Image>();
            _rectTrm = transform as RectTransform;
            originPos = _rectTrm.localPosition;
            _graphicRaycaster = _gr;
            cardData = data;
            CardVisual.Initialize(this);
            _mainCam = Camera.main;
            _cardHolder = holder;
            ChangeSprite = GetComponentInChildren<ChangeSprite>();
            ChangeSprite.Init(this);
            indexChange = false;
            _place = FindObjectOfType<CardPlace>();
        }
        public void Initialize(CardDataSO data)
        {
            cardData = data;
            CardVisual = GetComponentInChildren<CardVisual>();
            _imageCompo = GetComponent<Image>();
            _place = FindObjectOfType<CardPlace>();
            _rectTrm = transform as RectTransform;
            originPos = _rectTrm.localPosition;
            ChangeSprite = GetComponentInChildren<ChangeSprite>();
            ChangeSprite.Init(this);

            CardVisual.InitializeNoEvent(this);
            _imageCompo.raycastTarget = false;
        }

        private void Update()
        {
            DragFollow();
        }

        public bool UseCard()
        {
            if (TurnManager.Instance.firstUse ? !_place.CanUseOther(cardData) : !_place.CanUse(cardData)) return false;
            canDrag = false;

            _cardHolder.UseCard(this);
            _place.UseCard(this);
            Shy.ArtifactManager.Instance.OnEvent(Shy.EVENT_TYPE.USE_CARD);

            SoundManager.Instance.PlayEffect(_useCardClip);
            return true;
        }

        public void Flip(bool front)
        {
            CardVisual.Flip(front);
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
            CardVisual.SwapAnimation(dir);
        }

        #region Interface
        public void OnPointerEnter(PointerEventData eventData)
        {
            Explain.Instance.ShowExplain(cardData.specialShape, gameObject);
            PointerEnterEvent?.Invoke();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            Explain.Instance.HideExplain();
            PointerExitEvent?.Invoke();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            Explain.Instance.HideExplain();
            if (!canDrag) return;
            //if not mouse left click, then return
            if (eventData.button != PointerEventData.InputButton.Left) return;

            PointerDownEvent?.Invoke();
            _lastPointerDownTime = Time.time;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            Explain.Instance.ShowExplain(cardData.specialShape, gameObject);
            if (!canDrag) return;
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
            if (!canDrag) return;
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
            if (!canDrag) return;
            EndDragEvent?.Invoke();

            _cardHolder.SelectCard = null;
            isDragging = false;
            _graphicRaycaster.enabled = true;

            if ((_place.transform.position - transform.position).sqrMagnitude <= 1.5f && _place.CanUse(cardData))
            {
                if(!UseCard())
                {
                    CardVisual.isSelected = false;
                    if (!indexChange)
                        transform.SetSiblingIndex(_slibingIndex);
                    else
                        _cardHolder.SortingLayerOrder();
                    _cardHolder.ApplyLayoutWithTween(0.3f);
                    _rectTrm.DOLocalMoveY(originPos.y, 0.3f);
                }
            }
            else
            {
                CardVisual.isSelected = false;
                if (!indexChange)
                    transform.SetSiblingIndex(_slibingIndex);
                else
                    _cardHolder.SortingLayerOrder();
                _cardHolder.ApplyLayoutWithTween(0.3f);
                _rectTrm.DOLocalMoveY(originPos.y, 0.3f);
            }
            _imageCompo.raycastTarget = true;
            Explain.Instance.HideExplain();
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (!canDrag) return;
        }
        #endregion
    }

}
