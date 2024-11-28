using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Karin
{

    public class CardHolder : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private InputReaderSO _inputReader;
        [SerializeField] private Transform _cardSpawnTrm;
        [SerializeField] private GraphicRaycaster _graphicRaycaster;
        [SerializeField] private float _xPadding;
        public float layoutXDelta;

        public CardBase SelectCard
        {
            set
            {
                _selectCard = value;
            }
        }
        [SerializeField] private CardBase _selectCard;

        [SerializeField] private List<CardBase> cards = new List<CardBase>();
        [SerializeField] private List<float> layouts = new List<float>();

        [Header("Prefabs")]
        [SerializeField] private CardBase _cardPrefabs;

        [Header("Cards")]
        public List<CardDataSO> myCards;

        private RectTransform _rectTrm;
        private float holderWidth;
        private bool _firstUse = false;

        private void Awake()
        {
            _rectTrm = transform as RectTransform;
            holderWidth = _rectTrm.sizeDelta.x - _xPadding;
            CardDrag(false);
        }

        private void Update()
        {
            MoveLayout();
        }

        public void AddCard()
        {
            AddCard(GameManager.Instance.cardPack.GetCardData());
        }

        public void ChangeSpecialType(CardDataSO ChangeCard)
        {
            int idx = myCards.FindIndex(card => card.count == ChangeCard.count && card.shape == ChangeCard.shape);
            Debug.Log($"CardChanged : {ChangeCard.count} & {ChangeCard.shape} / {myCards[idx].specialShape} > {ChangeCard.specialShape}");
            myCards[idx].specialShape = ChangeCard.specialShape;
        }

        public void AddCard(CardDataSO data)
        {
            CardBase cb = Instantiate(_cardPrefabs, _cardSpawnTrm);
            cb.Initialize(data, this, _graphicRaycaster);
            cb.canDrag = true;
            cards.Insert(0, cb);
            AddLayout();
            SortingLayerOrder();
            ApplyLayoutWithTween(0.4f, 1);
            RectTransform rectTrm = cb.transform as RectTransform;

            float leftPos = -holderWidth / 2;
            float lengthDelta = holderWidth / (layouts.Count + 1);

            rectTrm.localPosition = new Vector3(leftPos - 300, 0, 0);
            rectTrm.DOLocalMoveX(leftPos + lengthDelta, 0.4f).SetEase(Ease.InExpo);
        }
        public void UseCard(CardBase card)
        {
            cards.Remove(card);
            RemoveLayout();
            SortingLayerOrder();
            ApplyLayoutWithTween(0.4f, 1);

            TurnManager.Instance.useCard = true;
            TurnManager.Instance.firstUse = true;

            if (card.cardData.IsThis(SpecialShapeType.King))
            {
                TurnManager.Instance.firstUse = false;
                TurnManager.Instance.useCard = false;
            }

            if (cards.Count <= 0)
            {
                StartSettings(() =>
                {
                    CardDrag(false);
                    TurnManager.Instance.Attack(10);
                });

            }

            ApplyLayoutWithTween(.3f);
        }
        public void MoveLayout()
        {
            if (_selectCard == null) return;

            int cardIdx = cards.FindIndex(x => x == _selectCard);
            float offset = layouts[cardIdx];
            Vector3 position = _selectCard.transform.localPosition;

            if (Mathf.Abs(offset - position.x) > layoutXDelta)
            {
                if (offset + position.x < offset && cardIdx > 0) //¿ÞÂÊ
                {
                    cards[cardIdx - 1].Swap(1);
                    (cards[cardIdx], cards[cardIdx - 1]) = (cards[cardIdx - 1], cards[cardIdx]);
                }

                else if (offset + position.x > offset && cardIdx < cards.Count - 1) //¿À¸¥ÂÊ
                {
                    cards[cardIdx + 1].Swap(-1);
                    (cards[cardIdx], cards[cardIdx + 1]) = (cards[cardIdx + 1], cards[cardIdx]);
                }

                _selectCard.indexChange = true;
                ApplyLayoutWithTween(.3f);
            }
        }

        private void AddLayout()
        {
            layouts.Add(1000);
            SortingLayout();
        }
        private void RemoveLayout()
        {
            layouts.RemoveAt(layouts.Count - 1);
            SortingLayout();
        }
        public void SortingLayerOrder()
        {
            if (cards.Count < 0 || cards == null) return;

            int cardCount = cards.Count;
            for (int i = 0; i < cardCount; i++)
            {
                cards[i].transform.SetSiblingIndex(i);
                cards[i].gameObject.name = $"Card [{i}]";
            }
        }
        public void SortingLayout()
        {
            if (layouts.Count < 0 || layouts == null) return;
            int count = layouts.Count;

            layoutXDelta = holderWidth / (count + 1);
            float leftPos = -holderWidth / 2;
            float prev = leftPos;

            for (int i = 0; i < layouts.Count; i++)
            {
                layouts[i] = prev + layoutXDelta;
                prev = layouts[i];
            }
        }
        public void ApplyLayoutWithTween(float time, int startIdx = 0)
        {
            int len = Mathf.Min(layouts.Count, cards.Count);

            for (int i = startIdx; i < len; i++)
            {
                float temp = layouts[i];
                if (cards[i].isDragging) continue;
                (cards[i].transform as RectTransform).DOLocalMoveX(temp, time);
            }
        }

        public void CardDrag(bool state)
        {
            foreach (var c in cards)
            {
                c.canDrag = state;
            }
        }
        public void StartSettings(Action callback)
        {
            StartCoroutine(StartSettingCoroutine(callback));
        }

        private IEnumerator StartSettingCoroutine(Action callback)
        {
            for (int i = 0; i < 5; i++)
            {
                AddCard(GameManager.Instance.cardPack.GetCardData());
                yield return new WaitForSeconds(0.2f);
            }
            CardDrag(true);
            if (callback != null) callback?.Invoke();
        }

        public void Release()
        {
            cards.ForEach(c => Destroy(c.gameObject));
            cards.Clear();
            layouts.Clear();
        }
    }

}