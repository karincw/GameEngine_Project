using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Karin
{

    public class EnemyCardHolder : MonoBehaviour
    {

        [SerializeField] private List<CardBase> cards = new List<CardBase>();
        [SerializeField] private List<float> layouts = new List<float>();
        public float layoutXDelta;
        [SerializeField] private Transform _cardSpawnTrm;
        [SerializeField] private CardBase _cardPrefabs;
        [SerializeField] private float _xPadding;
        [SerializeField] private float _cardMoveTime;


        [Header("AttackSettinges")]
        [SerializeField] private float _delayTime;

        private CardPlace _cardPlace;
        private RectTransform _rectTrm;
        private float holderWidth;

        private void Awake()
        {
            _rectTrm = transform as RectTransform;
            holderWidth = _rectTrm.sizeDelta.x - _xPadding;
        }
        private void Start()
        {
            _cardPlace = GameManager.Instance.cardPlace;
        }

        [ContextMenu("TADD")]
        public void AddCard()
        {
            AddCard(GameManager.Instance.cardPack.GetCardData());
        }

        public void AddCard(CardDataSO data)
        {
            CardBase cb = Instantiate(_cardPrefabs, _cardSpawnTrm);
            cb.Initialize(data);
            cb.canDrag = true;
            cards.Insert(0, cb);
            AddLayout();
            SortingLayerOrder();
            ApplyLayoutWithTween(_cardMoveTime, 1);
            RectTransform rectTrm = cb.transform as RectTransform;

            float leftPos = -holderWidth / 2;
            float lengthDelta = holderWidth / (layouts.Count + 1);

            rectTrm.localPosition = new Vector3(leftPos - 300, 0, 0);
            rectTrm.DOLocalMoveX(leftPos + lengthDelta, _cardMoveTime).SetEase(Ease.InExpo);
        }
        public void UseCard(CardBase card)
        {
            cards.Remove(card);
            RemoveLayout();
            SortingLayerOrder();
            ApplyLayoutWithTween(_cardMoveTime, 1);

            if (card.cardData.count == CountType.ACE || card.cardData.count == CountType.Two)
                TurnManager.Instance.hitInfo.nowhit = false;

            if (cards.Count <= 0)
            {
                StartSettings();
                TurnManager.Instance.Attack(10);
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
        public void ApplyLayout(int startIdx = 0)
        {
            int len = Mathf.Min(layouts.Count, cards.Count);

            for (int i = startIdx; i < len; i++)
            {
                float temp = layouts[i];
                if (cards[i].isDragging) continue;
                (cards[i].transform as RectTransform).localPosition = new Vector2(temp, 0);
            }
        }
        public void StartSettings()
        {
            StartCoroutine(StartSettingCoroutine());
        }
        private IEnumerator StartSettingCoroutine()
        {
            for (int i = 0; i < 5; i++)
            {
                AddCard(GameManager.Instance.cardPack.GetCardData());
                yield return new WaitForSeconds(0.2f);
            }
        }

        public void AutoRun()
        {
            StartCoroutine(AutoRunCoroutine());
        }

        private IEnumerator AutoRunCoroutine()
        {
            CardBase selectedCard = ClacluateCard();
            SwapCard(selectedCard);
            ApplyLayout();
            SortingLayerOrder();

            yield return new WaitForSeconds(_delayTime);

            if (selectedCard == null)
            {
                TurnManager.Instance.ChangeTurn();
                yield break;
            }

            TurnManager.Instance.useCard = true;
            selectedCard.Flip(true);
            UseCard(selectedCard);
            _cardPlace.UseCard(selectedCard);

            if (selectedCard.cardData.count == CountType.King)
            {
                TurnManager.Instance.useCard = false;
                AutoRun();
                yield break;
            }

            if (selectedCard.cardData.count == CountType.ACE || selectedCard.cardData.count == CountType.Two)
                TurnManager.Instance.hitInfo.nowhit = false;

            TurnManager.Instance.ChangeTurn();
        }

        public CardBase ClacluateCard()
        {
            List<CardBase> cardDatas = new();
            foreach (var card in cards)
            {
                if (_cardPlace.CanUse(card.cardData))
                    cardDatas.Add(card);
            }

            if (cardDatas.Count <= 0)
                return null;

            if (TurnManager.Instance.hitInfo.nowhit == true) // 공격받은상태임
            {
                bool howToDeal = Convert.ToBoolean(Random.Range(0, 1));
                if (howToDeal) // 공격으로 처리해볼래
                {
                    List<CardBase> attackCards = cardDatas.Where(c => c.cardData.IsAttackCard).ToList();
                    if (attackCards.Count > 0)
                    {
                        return attackCards[Random.Range(0, attackCards.Count)];
                    }
                    List<CardBase> defenceCards = cardDatas.Where(c => c.cardData.IsDefenceCard).ToList();
                    if (defenceCards.Count > 0)
                    {
                        return defenceCards[Random.Range(0, defenceCards.Count)];
                    }
                }
                else
                {
                    List<CardBase> attackCards = cardDatas.Where(c => c.cardData.IsAttackCard).ToList();
                    if (attackCards.Count > 0)
                    {
                        return attackCards[Random.Range(0, attackCards.Count)];
                    }
                    List<CardBase> defenceCards = cardDatas.Where(c => c.cardData.IsDefenceCard).ToList();
                    if (defenceCards.Count > 0)
                    {
                        return defenceCards[Random.Range(0, defenceCards.Count)];
                    }
                }
            }

            if (cardDatas.FirstOrDefault(c => c.cardData.count == CountType.King) != null)
            {
                return cardDatas.FirstOrDefault(c => c.cardData.count == CountType.King);
            }

            return cardDatas[Random.Range(0, cardDatas.Count)];

        }
        public void SwapCard(CardBase card)
        {
            if (card == null) return;
            int swapIdx = cards.Count - 1;
            int findIdx = cards.FindIndex(c => c.Equals(card));

            if (findIdx == -1)
            {
                Debug.LogError("SwapCard <= CANNOT FIND CARDDATA");
                return;
            }

            (cards[swapIdx], cards[findIdx]) = (cards[findIdx], cards[swapIdx]);

        }

    }

}