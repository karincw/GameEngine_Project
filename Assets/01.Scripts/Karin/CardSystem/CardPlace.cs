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

        private ChangeSprite _current;


        public bool CanUse(CardDataSO c)
        {
            if (c == null) return false;

            if (TurnManager.Instance.hitInfo.hit)
            {
                if (
                    (c.count == firstCardData.count || ((int)c.shape == (int)firstCardData.shape)) //모양이 같거나 숫자가 같음
                    && (c.IsAttackCard() || c.IsDefenceCard())) //공격카드이거나 수비 카드임
                {
                    return true;
                }
            }
            else if (c.count == firstCardData.count || ((int)c.shape == (int)firstCardData.shape))
            {
                return true;
            }

            return false;
        }

        public void UseCard(CardBase card)
        {
            if (_current != null) _current.SetOriginShape();

            cards.Add(card);
            card.transform.SetParent(transform);
            Vector2 positionDelta = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            card.transform.DOLocalMove(positionDelta, 0.1f);

            CardManager.Instance.ApplyCardEffect(card.cardData);

            if (card.cardData.specialShape == SpecialShapeType.ChangeShape)
            {
                if (TurnManager.Instance.currentTurn == Turn.Player)
                {
                    _current = card.ChangeSprite;
                    _current.SelectStart();
                }
                else
                {
                    _current = card.ChangeSprite;
                    _current.Selected(GameManager.Instance.EnemyCardHolder.GetNeedShape());
                }
            }
        }

        public List<CardBase> GetCards()
        {
            var card = cards.GetRange(0, cards.Count - 5);
            cards.RemoveRange(0, cards.Count - 5);
            return card;
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

        public void Release()
        {
            cards.ForEach(c => Destroy(c.gameObject));
            cards.Clear();
        }
    }

}