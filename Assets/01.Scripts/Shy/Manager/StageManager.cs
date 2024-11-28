using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Karin;
using UnityEngine.UI;
using TMPro;

namespace Shy
{
    public enum ATTACK_TYPE
    {
        ATTACK,
        HEAL
    }

    public class StageManager : MonoSingleton<StageManager>
    {
        [SerializeField, Header("STAGE")] private StageListSO stageSO;
        [SerializeField] private List<Stage> nowMap;


        [SerializeField, Header("ITEM")] private Transform cardAnimeStart;
        [SerializeField] private Transform selectorPos;
        private SelectorItem curSelectItem;

        [SerializeField] private CardBase enemyCardPrefab;
        [SerializeField] private Transform enemyCardUi;

        [SerializeField, Header("DISPLAY")] private Transform displayAnime;
        [SerializeField] private Transform display;
        [SerializeField] private Transform displayPos;

        [SerializeField, Header("NAMECARD")] private EnemyData playerNormalSO;
        public Selector_Enemy playerNameCard;
        public Selector_Enemy enemyNameCard;
        [SerializeField] private Button _startBt;

        private bool canUseItem = false;
        private CardBase[] enemyHaveCards = new CardBase[31];

        [SerializeField, Header("BATTLE")] private GameObject battleUI;


        public void ResetArtifact(Selector_Enemy _nameCard = null)
        {
            if (_nameCard == null) _nameCard = playerNameCard;

            Transform pos = _nameCard.transform.GetChild(0).Find("Artifact");

            for (int i = pos.childCount; i > 0; i--)
            {
                Destroy(pos.GetChild(0).gameObject);
            }

            _nameCard.artifacts.RemoveRange(0, _nameCard.artifacts.Count);
        }

        public void AddArtifact(ArtifactData _art, Selector_Enemy _nameCardPos = null)
        {
            Debug.Log(_nameCardPos);

            if (_nameCardPos == null) _nameCardPos = playerNameCard;

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
            StartCoroutine(ItemAnime());
        }

        public IEnumerator ItemAnime()
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
            enemyCardUi.gameObject.SetActive(true);

            if (_item is Selector_Enemy)
            {
                Selector_Enemy item = _item as Selector_Enemy;
                if (item.data.cardDeck.Count != 0)
                {
                    enemyCardUi.GetChild(1).gameObject.SetActive(true);
                    enemyCardUi.GetChild(0).gameObject.SetActive(false);

                    for (int i = 0; i < 30; i++)
                    {
                        if (i < item.data.cardDeck.Count)
                        {
                            enemyHaveCards[i].gameObject.SetActive(true);
                            enemyHaveCards[i].cardData = item.data.cardDeck[i];
                            CardDataSO so = enemyHaveCards[i].cardData;

                            Transform visual = enemyHaveCards[i].transform.GetChild(0);
                            Color32 color;

                            if (so.specialShape == (SpecialShapeType)so.shape)
                                color = CardManager.Instance.ShapeToColorDictionary[(SpecialShapeType)so.shape];
                            else
                                color = Color.white;

                            visual.GetChild(0).GetComponent<Image>().color = color;
                            visual.GetChild(0).GetComponent<Image>().sprite = CardManager.Instance.ShapeToSpriteDictionary[enemyHaveCards[i].cardData.specialShape];

                            TMP_FontAsset font;
                            Sprite sprite;

                            if (so.shape == BaseShapeType.Diamond || so.shape == BaseShapeType.Heart)
                            {
                                font = CardManager.Instance.PinkFont;
                                color = Color.red;
                                sprite = CardManager.Instance.ShapeToSpriteDictionary[(SpecialShapeType)so.shape];
                            }
                            else
                            {
                                font = CardManager.Instance.BlackFont;
                                color = Color.black;
                                sprite = CardManager.Instance.ShapeToSpriteDictionary[(SpecialShapeType)so.shape];
                            }

                            visual.GetChild(1).GetComponent<Image>().sprite = sprite;
                            visual.GetChild(1).GetComponent<Image>().color = color;
                            visual.GetChild(2).GetComponent<Image>().sprite = sprite;
                            visual.GetChild(2).GetComponent<Image>().color = color;

                            visual.GetChild(3).GetComponent<TextMeshProUGUI>().text = CardManager.Instance.GetCountText(so.count);
                            visual.GetChild(3).GetComponent<TextMeshProUGUI>().font = font;
                            visual.GetChild(4).GetComponent<TextMeshProUGUI>().text = CardManager.Instance.GetCountText(so.count);
                            visual.GetChild(4).GetComponent<TextMeshProUGUI>().font = font;
                        }
                        else
                            enemyHaveCards[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    enemyCardUi.transform.GetChild(0).gameObject.SetActive(true);
                    enemyCardUi.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }

        public void EnemySelect()
        {
            enemyCardUi.gameObject.SetActive(false);
            while (selectorPos.childCount != 0)
                SelectorPooling.Instance.ReturnPool(selectorPos.GetComponentInChildren<SelectorItem>());

            enemyNameCard.Init((curSelectItem as Selector_Enemy).data);
            //여기서 전투 시작 함수

            SoundManager.Instance.PlayBGM((curSelectItem as Selector_Enemy).data.audio);
            StartCoroutine(StartGameCoroutine());
        }

        private IEnumerator StartGameCoroutine()
        {
            DisplayManager.Instance.SignUpdate("");
            battleUI.SetActive(true);
            display.DOMoveY(displayAnime.position.y, 0.8f);

            ArtifactManager.Instance.ArtifactsInit();

            yield return new WaitForSeconds(1f);

            ArtifactManager.Instance.OnEvent(EVENT_TYPE.STAGE_START, EVENT_TYPE.STAGE_START, (curSelectItem as Selector_Enemy).data);
        }

        public void EnemyCancel()
        {
            curSelectItem = null;
            enemyCardUi.gameObject.SetActive(false);
        }
        #endregion

        public void StageUpdate()
        {
            StartCoroutine(Updating());
        }

        private IEnumerator Updating()
        {
            Debug.Log("StageUpdate : " + nowMap[0].mapType);
            DisplayManager.Instance.SignUpdate(nowMap[0].mapType == MAP_TYPE.BATTLE ? "WHO'S NEXT?" : "CHOOSE ONE");


            yield return new WaitForSeconds(0.7f);

            if (nowMap[0].mapType != MAP_TYPE.EVENT) SetItem();
        }

        [SerializeField] private GameObject End;
        public void StageClear()
        {
            SelectorPooling.Instance.ReturnPool(selectorPos.GetComponentsInChildren<SelectorItem>());
            ExplainManager.Instance.HideExplain();

            if (nowMap.Count == 1)
            {
                Debug.Log("clear");
                End.SetActive(true);
                return;
            }

            nowMap.RemoveAt(0);
            StageUpdate();
        }

        public void StageInit()
        {
            //display
            DisplayManager.Instance.SignUpdate("");

            End.SetActive(false);

            //playerCard
            playerNameCard.Init(playerNormalSO);
            playerNameCard.transform.GetChild(0).transform.DOMoveY(-10, 0);

            //enemyCard
            enemyNameCard.gameObject.SetActive(false);

            //Map
            nowMap = new List<Stage>(stageSO.stageList);

            battleUI.SetActive(false);
            _startBt.interactable = true;
            
            //display
            display.DOMoveY(displayPos.position.y, 0.7f).OnComplete(() => DisplayManager.Instance.SignUpdate("Own Card"));
        }

        private void Start()
        {
            DisplayManager.Instance.SignUpdate("");
            for (int i = 0; i < 30; i++)
            {
                Instantiate(enemyCardPrefab, enemyCardUi.GetChild(1));
            }
            enemyHaveCards = enemyCardUi.GetChild(1).GetComponentsInChildren<CardBase>();
            StageInit();
        }

        public void GameStart()
        {
            _startBt.interactable = false;

            playerNameCard.transform.GetChild(0).gameObject.SetActive(true);
            playerNameCard.transform.GetChild(0).DOLocalMoveY(100, 1f).OnComplete(() => StageUpdate());
        }

        public void GameFin()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(enemyNameCard.transform.GetChild(0).DOMoveY(8, 1.5f).OnComplete(() =>
            {
                enemyNameCard.gameObject.SetActive(false);
                enemyNameCard.transform.GetChild(0).DOLocalMoveY(-100, 0);
            }));

            SoundManager.Instance.StopBGM();

            battleUI.SetActive(false);
            Karin.GameManager.Instance.ReleaseGame();

            if (playerNameCard.health <= 0)
            {
                //플레이어 죽는 거
                Debug.Log("enemy win");
                
                seq.Append(display.DOMoveY(displayPos.position.y, 0.7f));
                seq.OnComplete(() => DisplayManager.Instance.DieSign());
            }
            else if (enemyNameCard.health <= 0)
            {
                Debug.Log("player win");
                int vlu = (curSelectItem as Selector_Enemy).data.life - 5;

                seq.Append(display.DOMoveY(displayPos.position.y, 0.7f).OnStart(()=>
                Damage((vlu >= 5 ? vlu : 5), Turn.Enemy, ATTACK_TYPE.HEAL, false)));
                seq.OnComplete(() => StageClear());
            }
        }


        public bool Damage(int _value, Turn _turn, ATTACK_TYPE _atkType = ATTACK_TYPE.ATTACK, bool cardEffect = true)
        {
            _value *= (_atkType == ATTACK_TYPE.ATTACK) ? -1 : 1;

            if (_turn == Turn.Enemy)
            {
                DamageEffect.Instance.Damage(_value, playerNameCard, cardEffect);
                return playerNameCard.health - _value <= 0;
            }
            else if (_turn == Turn.Player)
            {
                DamageEffect.Instance.Damage(_value, enemyNameCard, cardEffect);
                return enemyNameCard.health - _value <= 0;
            }

            return false;
        }
    }
}
