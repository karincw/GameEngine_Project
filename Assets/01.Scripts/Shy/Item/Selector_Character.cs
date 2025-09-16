using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using CardDataSO = Karin.CardDataSO;

namespace Shy
{
    public class Selector_Character : SelectorItem
    {
        internal EnemyData data;
        public List<CardDataSO> cardDataSoList;
        public List<Artifact> artifacts;

        [SerializeField] private TextMeshProUGUI namePos, lifePos;
        [SerializeField] private bool isButton = true;

        private int Health;
        public int health
        {
            get => Health;
            set { Health = value; lifePos.text = Health.ToString(); }
        }

        public override void Init(Item_DataSO _base)
        {
            data = _base as EnemyData;

            health = data.life;
            namePos.text = data.itemName;

            cardDataSoList = new List<CardDataSO>();
            foreach (EnemyHaveCard _item in data.cardDeck)
            {
                cardDataSoList.Add(new CardDataSO(Karin.CardType.Gold, _item._count, _item._shape, _item._sticker.shape));
            }

            GameManager.Instance.ResetArtifact(this);
            gameObject.SetActive(true);

            for (int i = 0; i < data.artifacts.Count; i++)
                GameManager.Instance.AddArtifact(data.artifacts[i], this);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (isButton) GameManager.Instance.ItemChoose(this);
        }
    }
}
