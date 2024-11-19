using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Karin;
using UnityEngine.UI;
using TMPro;

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
        private CardBase[] enemyHaveCards = new CardBase[31];



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
                SelectorItem item = SelectorPooling.Instance.GetPool(c.iType);

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
                Selector_Enemy item = _item as Selector_Enemy;
                if (item.data.cardDeck.Count != 0)
                {
                    lastChooseUI.transform.GetChild(1).gameObject.SetActive(true);
                    lastChooseUI.transform.GetChild(0).gameObject.SetActive(false);

                    Debug.Log(lastChooseUI.transform.GetChild(1).gameObject.name);
                    Debug.Log(lastChooseUI.transform.GetChild(1).GetComponentInChildren<CardBase>());
                    if (enemyHaveCards[0] == null) 
                        enemyHaveCards = lastChooseUI.transform.GetChild(1).GetComponentsInChildren<CardBase>();

                    for (int i = 0; i < 30; i++)
                    {
                        if(i < item.data.cardDeck.Count)
                        {
                            enemyHaveCards[i].gameObject.SetActive(true);
                            enemyHaveCards[i].cardData = item.data.cardDeck[i];

                            Transform visual = enemyHaveCards[i].transform.GetChild(0);

                            visual.GetChild(0).GetComponent<Image>().sprite = CardManager.Instance.ShapeToSpriteDictionary[enemyHaveCards[i].cardData.specialShape];

                            visual.GetChild(1).GetComponent<Image>().color = 
                                CardManager.Instance.ShapeToColorDictionary[(SpecialShapeType)enemyHaveCards[i].cardData.shape];
                            visual.GetChild(2).GetComponent<Image>().color =
                                CardManager.Instance.ShapeToColorDictionary[(SpecialShapeType)enemyHaveCards[i].cardData.shape];

                            ColorUpdate(visual.GetChild(3).GetComponent<TextMeshProUGUI>(), enemyHaveCards[i]);
                            ColorUpdate(visual.GetChild(4).GetComponent<TextMeshProUGUI>(), enemyHaveCards[i]);
                        }
                        else
                            enemyHaveCards[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    lastChooseUI.transform.GetChild(0).gameObject.SetActive(true);
                    lastChooseUI.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }

        public void ColorUpdate(TextMeshProUGUI _tmp, CardBase _s)
        {
            _tmp.text = CardManager.Instance.GetCountText(_s.cardData.count);
            _tmp.font = (_s.cardData.specialShape is SpecialShapeType.Diamond or
                SpecialShapeType.Heart) ? CardManager.Instance.PinkFont : CardManager.Instance.BlackFont;
        }

        public void EnemySelect()
        {
            lastChooseUI.SetActive(false);
            while (selectorPos.childCount != 0)
                SelectorPooling.Instance.ReturnPool(selectorPos.GetComponentInChildren<SelectorItem>());

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
            DisplaySign.Instance.SignUpdate(nowMap[0].mapType == MAP_TYPE.BATTLE ? "WHO'S NEXT?" : "CHOOSE ONE");

            if(nowMap[0].mapType != MAP_TYPE.EVENT) SetItem();
        }

        public void StageClear()
        {
            if(nowMap.Count == 1)
            {
                Debug.Log("�� �̻� ���� �����ϴ�. ��");
                return;
            }

            SelectorPooling.Instance.ReturnPool(selectorPos.GetComponentsInChildren<SelectorItem>());

            ExplainManager.Instance.HideExplain();

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
