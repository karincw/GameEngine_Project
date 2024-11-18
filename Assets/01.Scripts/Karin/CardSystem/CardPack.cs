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
            cards.AddRange(datas);
            Shuffle();
        }

        public CardDataSO GetCardData()
        {
            int idx = cards.Count - 1;
            CardDataSO rcard = cards[idx];
            if (cards.Count <= 10)
            {
                SetCards(_place.GetCards().Select(c => c.cardData).ToList());
            }
            cards.RemoveAt(idx);
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