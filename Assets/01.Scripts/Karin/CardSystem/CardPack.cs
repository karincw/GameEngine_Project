using System;
using System.Collections;
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

        public void SetCards(params HashSet<CardDataSO>[] hashs)
        {
            HashSet<CardDataSO> hashSet = new HashSet<CardDataSO>();
            foreach (var hash in hashs)
            {
                hashSet.UnionWith(hash);
            }
            cards = hashSet.ToList();
        }

        public void AddCard()
        {

        }

        public CardDataSO GetCardData()
        {
            CardDataSO rcard = cards.Last();
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