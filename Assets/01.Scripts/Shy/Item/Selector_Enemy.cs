using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shy
{
    public class Selector_Enemy : SelectorItem
    {
        public EnemyData data;
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

            gameObject.SetActive(true);

            Debug.Log("Enemy init");
            for (int i = 0; i < data.artifacts.Count; i++)
                StageManager.Instance.AddArtifact(data.artifacts[i], this);
        }

        public void GetDamage()
        {

        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (isButton == false) return;

            StageManager.Instance.EnemyChoose(this);
        }
    }
}
