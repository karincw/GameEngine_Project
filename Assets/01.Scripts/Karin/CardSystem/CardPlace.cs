using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Karin
{

    public class CardPlace : MonoBehaviour
    {
        public List<CardBase> cards = new List<CardBase>();
        private CardDataSO firstCardData => cards[0].cardData;

        [SerializeField] private Outline _outLine;
        [SerializeField] private CardPack _pack;
        [SerializeField] private CardBase _cardPrefabs;


        public bool CanUse(CardDataSO c)
        {
            if (c == null) return false;

            if (c.count.Equals(firstCardData.count) || ((int)c.shape & (int)firstCardData.shape) > 0)
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
        }

        [ContextMenu("StartSettings")]
        public void FirstCardSetting()
        {
            CardBase card = Instantiate(_cardPrefabs, transform);
            card.Initialize(_pack.GetCardData());
            cards.Add(card);
        }
    }

}