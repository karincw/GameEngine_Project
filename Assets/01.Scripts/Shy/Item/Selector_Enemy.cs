using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using CardDataSO = Karin.CardDataSO;

namespace Shy
{
    public class Selector_Enemy : SelectorItem
    {
        internal EnemyData data;
        public List<CardDataSO> cardDataSoList;

        [SerializeField] private TextMeshProUGUI namePos;
        [SerializeField] private TextMeshProUGUI lifePos;
        [SerializeField] private int Health;
        [SerializeField] private bool isButton = true;

        public int health
        {
            get => Health;
            set { Health = value; lifePos.text = Health.ToString(); }
        }
        public List<Artifact> artifacts;


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
            if (isButton == false) return;

            GameManager.Instance.ItemChoose(this);
        }
    }
}
