using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Karin
{

    public class CardPack : MonoBehaviour
    {
        public List<CardDataSO> cards = new List<CardDataSO>();

        [SerializeField] private CardPlace _place;

        public void SetCards(List<CardDataSO> datas)
        {
            Debug.Log("CardAdd");
            cards.AddRange(datas);
            Shuffle();
        }

        public CardDataSO GetCardData()
        {
            //int idx = cards.Count - 1;
            CardDataSO rcard = cards[^1];
            if (cards.Count <= 10)
            {
                var cards = _place.GetCards();
                SetCards(cards.Select(c => c.cardData).ToList());
                cards.ForEach(c => Destroy(c));
            }
            cards.Remove(rcard);
            return rcard;
        }

        [ContextMenu("Shuffle")]
        private void Shuffle()
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                CardDataSO value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }
    }

}