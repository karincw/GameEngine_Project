using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Karin
{

    public class CardPlace : MonoBehaviour
    {
        public List<CardBase> cards = new List<CardBase>();
        private CardDataSO firstCardData => cards[cards.Count - 1].cardData;

        [SerializeField] private Outline _outLine;
        [SerializeField] private CardPack _pack;
        [SerializeField] private CardBase _cardPrefabs;


        public bool CanUse(CardDataSO c)
        {
            if (c == null) return false;

            if (c.count == firstCardData.count || ((int)c.shape == (int)firstCardData.shape))
            {
                return true;
            }

            return false;
        }

        public void UseCard(CardBase card)
        {
            cards.Add(card);
            card.transform.SetParent(transform);
            Vector2 positionDelta = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            card.transform.DOLocalMove(positionDelta, 0.1f);

            TurnManager.Instance.Attack(CardManager.Instance.GetDamage(card.cardData.count));
        }

        public List<CardBase> GetCards()
        {
            return cards.GetRange(0, cards.Count - 5);
        }

        [ContextMenu("StartSettings")]
        public void CardSetting()
        {
            CardBase card = Instantiate(_cardPrefabs, transform);
            RectTransform cardRect = card.transform as RectTransform;
            cardRect.localPosition = new Vector3(248, 22, 0);
            card.Initialize(_pack.GetCardData());

            card.Flip(true);
            cardRect.DOLocalMove(Vector3.zero, 0.8f);

            cards.Add(card);
        }
    }

}