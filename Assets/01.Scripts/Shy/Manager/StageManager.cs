using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;


namespace Shy
{
    public class StageManager : MonoSingleton<StageManager>
    {
        [SerializeField] private Transform selectorPos;
        private SelectorItem curSelectItem;
        public GameObject lastChooseUI;

        [SerializeField, Header("STAGE")] private StageListSO stageSO;
        [SerializeField] private List<Stage> nowMap;
        [SerializeField] private Transform cardAnimeStart;

        [SerializeField] private EnemyData playerNormalSO;
        public Selector_Enemy playerCard;
        public Selector_Enemy enemyCard;

        private bool canUseItem = false;



        public void AddArtifact(ArtifactData _art, Selector_Enemy _nameCardPos = null)
        {
            Debug.Log(_nameCardPos);

            if (_nameCardPos == null) _nameCardPos = playerCard;

            Artifact a = Instantiate(_art.artifact, _nameCardPos.transform.GetChild(0).Find("Artifact"));
            a.Init(_art);
            _nameCardPos.artifacts.Add(a);
        }

        #region ������ ����
        public void SetItem()
        {
            selectorPos.gameObject.SetActive(true);

            int rand = (int)Random.Range(Mathf.Min(nowMap[0].spawnCnt.x, nowMap[0].spawnCnt.y), 
                Mathf.Max(nowMap[0].spawnCnt.x, nowMap[0].spawnCnt.y) + 1);

            //Pool
            List<Item_DataSO> cList = new List<Item_DataSO>();


            while (selectorPos.childCount < rand)
            {
                Item_DataSO c = nowMap[0].spawnItem[Random.Range(0, nowMap[0].spawnItem.Count)];
                SelectorItem item = SelectorPooling.lnstance.GetPool(c.iType);

                item.transform.parent = selectorPos;
                item.transform.GetChild(0).gameObject.SetActive(false);
                
                item.Init(c);
                nowMap[0].spawnItem.Remove(c);
                cList.Add(c);
            }

            for (int i = 0; i < cList.Count; i++)
                nowMap[0].spawnItem.Add(cList[i]);

            canUseItem = false;
            StartCoroutine(Anime());
        }

        public IEnumerator Anime()
        {
            yield return new WaitForEndOfFrame();

            Sequence seq = DOTween.Sequence();

            for (int i = 0; i < selectorPos.childCount; i++)
            {
                Transform visual = selectorPos.GetChild(i).Find("Visual");
                visual.position = cardAnimeStart.position;
                visual.gameObject.SetActive(true);

                seq.Append(visual.DOLocalMove(Vector3.zero, 0.8f));
            }

            seq.OnComplete(() => canUseItem = true);
        }

        public void EnemyChoose(SelectorItem _item)
        {
            curSelectItem = _item;
            lastChooseUI.SetActive(true);

            if(_item is Selector_Enemy)
            {
                if ((_item as Selector_Enemy).data.cardDeck.Count != 0)
                {
                    lastChooseUI.transform.GetChild(1).gameObject.SetActive(true);
                    lastChooseUI.transform.GetChild(0).gameObject.SetActive(false);


                }
                else
                {
                    lastChooseUI.transform.GetChild(0).gameObject.SetActive(true);
                    lastChooseUI.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }

        public void EnemySelect()
        {
            lastChooseUI.SetActive(false);
            while (selectorPos.childCount != 0)
                SelectorPooling.lnstance.ReturnPool(selectorPos.GetComponentInChildren<SelectorItem>());

            enemyCard.Init((curSelectItem as Selector_Enemy).data);
            //여기서 전투 시작 함수
        }

        public void EnemyCancel()
        {
            curSelectItem = null;
            lastChooseUI.SetActive(false);
        }
        #endregion


        public void StageUpdate()
        {
            Debug.Log("StageUpdate : " + nowMap[0].mapType);
            DisplaySign.lnstance.SignUpdate(nowMap[0].mapType == MAP_TYPE.BATTLE ? "WHO'S NEXT?" : "CHOOSE ONE");

            if(nowMap[0].mapType != MAP_TYPE.EVENT) SetItem();
        }

        public void StageClear()
        {
            if(nowMap.Count == 1)
            {
                Debug.Log("�� �̻� ���� �����ϴ�. ��");
                return;
            }

            SelectorPooling.lnstance.ReturnPool(selectorPos.GetComponentsInChildren<SelectorItem>());

            ExplainManager.lnstance.HideExplain();

            nowMap.RemoveAt(0);
            StageUpdate();
        }

        private void StageInit()
        {
            enemyCard.gameObject.SetActive(false);
            nowMap = new List<Stage>(stageSO.stageList);

            StageUpdate();
        }

        private void Start()
        {
            playerCard.Init(playerNormalSO);
            StageInit();
        }
    }
}
