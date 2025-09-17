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

    public class GameManager : MonoSingleton<GameManager>
    {
        #region Variable
        [SerializeField, Header("STAGE")] private StageListSO stageSO;
        [SerializeField] private List<Stage> stageList;

        [SerializeField, Header("ITEM")] private Transform cardAnimeStart;
        [SerializeField] private Transform selectorPos;
        private SelectorItem curSelectItem;

        [SerializeField] private CardBase enemyCardPrefab;
        [SerializeField] private Transform enemyCardUi;

        [SerializeField, Header("DISPLAY")] private Transform displayAnime;
        [SerializeField] private Transform display;
        [SerializeField] private Transform displayPos;

        [SerializeField, Header("NAME CARD")] private EnemyData playerNormalSO;
        public Selector_Character playerNameCard;
        public Selector_Character enemyNameCard;
        [SerializeField] private Button _startBt;

        private CardBase[] enemyHaveCards = new CardBase[31];

        [SerializeField] private GameObject End;

        [SerializeField, Header("BATTLE")] private GameObject battleUI;
        #endregion

        #region Artifact
        public void ResetArtifact(Selector_Character _nameCard = null)
        {
            if (_nameCard == null) _nameCard = playerNameCard;
            Transform pos = _nameCard.transform.GetChild(0).Find("Artifact");

            for (int i = pos.childCount; i > 0; i--) Destroy(pos.GetChild(0).gameObject);

            _nameCard.artifacts.RemoveRange(0, _nameCard.artifacts.Count);
        }

        public void AddArtifact(ArtifactData _art, Selector_Character _nameCardPos = null)
        {
            if (_nameCardPos == null) _nameCardPos = playerNameCard;

            Artifact a = Instantiate(_art.artifact, _nameCardPos.transform.GetChild(0).Find("Artifact"));
            a.Init(_art);
            _nameCardPos.artifacts.Add(a);
        }
        #endregion

        #region Item
        public void SetItem()
        {
            List<Item_DataSO> cList = new List<Item_DataSO>();
            selectorPos.gameObject.SetActive(true);

            int x = stageList[0].spawnCnt.x, y = stageList[0].spawnCnt.y;
            int _cnt = Random.Range(Mathf.Min(x, y), Mathf.Max(x, y) + 1);

            while (selectorPos.childCount < _cnt)
            {
                Item_DataSO _data = stageList[0].spawnItem[Random.Range(0, stageList[0].spawnItem.Count)];
                
                SelectorItem _item = SelectorPooling.Instance.GetPool(_data.iType);
                _item.transform.parent = selectorPos;
                _item.transform.GetChild(0).gameObject.SetActive(false);
                _item.Init(_data);

                stageList[0].spawnItem.Remove(_data);
                cList.Add(_data);
            }

            for (int i = 0; i < cList.Count; i++) stageList[0].spawnItem.Add(cList[i]);

            StartCoroutine(ItemAppearAnime());
        }

        public IEnumerator ItemAppearAnime()
        {
            yield return new WaitForEndOfFrame();

            Sequence seq = DOTween.Sequence();
            
            for (int i = 0; i < selectorPos.childCount; i++)
            {
                Transform visual = selectorPos.GetChild(i).Find("Visual");
                visual.position = cardAnimeStart.position;
                seq.Append(visual.DOLocalMove(Vector3.zero, 0.8f).OnStart(() => visual.gameObject.SetActive(true)));
            }
        }

        public void ItemChoose(SelectorItem _item)
        {
            curSelectItem = _item;
            if (curSelectItem is Selector_Character) EnemyChoose();
        }
        #endregion

        #region Enemy
        private void EnemyChoose()
        {
            Selector_Character item = curSelectItem as Selector_Character;
            int enemyCardCnt = item.cardDataSoList.Count;

            enemyCardUi.gameObject.SetActive(true);
            enemyCardUi.GetChild(1).gameObject.SetActive(enemyCardCnt != 0);
            enemyCardUi.GetChild(0).gameObject.SetActive(enemyCardCnt == 0);

            if (enemyCardCnt == 0) return;

            for (int i = 0; i < 30; i++)
            {
                enemyHaveCards[i].gameObject.SetActive(i < enemyCardCnt);

                if (i < enemyCardCnt)
                {
                    CardDataSO so = enemyHaveCards[i].cardData = item.cardDataSoList[i];
                    Transform paPos = enemyHaveCards[i].transform.GetChild(0);
                    Color32 _color = Color.white;
                    TMP_FontAsset _font;
                    Sprite _sprite = CardManager.Instance.ShapeToSpriteDictionary[enemyHaveCards[i].cardData.specialShape];

                    #region Center Sticker
                    if (so.specialShape == (SpecialShapeType)so.shape)
                        _color = CardManager.Instance.ShapeToColorDictionary[so.specialShape];

                    paPos.GetChild(0).GetComponent<Image>().color = _color;
                    paPos.GetChild(0).GetComponent<Image>().sprite = _sprite;
                    #endregion

                    #region Out Sticker
                    _font = (int)so.shape < 2 ? CardManager.Instance.PinkFont : CardManager.Instance.BlackFont;
                    _color = (int)so.shape < 2 ? Color.red : Color.black;
                    _sprite = CardManager.Instance.ShapeToSpriteDictionary[(SpecialShapeType)so.shape];

                    for (int j = 1; j < 3; j++)
                    {
                        Image v = paPos.GetChild(j).GetComponent<Image>();
                        v.sprite = _sprite;
                        v.color = _color;

                        TextMeshProUGUI txt = paPos.GetChild(j + 2).GetComponent<TextMeshProUGUI>();
                        txt.text = CardManager.Instance.GetCountText(so.count);
                        txt.font = _font;
                    }
                    #endregion
                }
            }
        }

        public void EnemySelect()
        {
            enemyCardUi.gameObject.SetActive(false);
            while (selectorPos.childCount != 0)
                SelectorPooling.Instance.ReturnPool(selectorPos.GetComponentInChildren<SelectorItem>());

            enemyNameCard.Init((curSelectItem as Selector_Character).data);

            SoundManager.Instance.PlayBGM((curSelectItem as Selector_Character).data.audio);
            StartCoroutine(StartBattleCoroutine());
        }

        public void EnemyCancel()
        {
            curSelectItem = null;
            enemyCardUi.gameObject.SetActive(false);
        }
        #endregion

        private void Start()
        {
            DisplayManager.Instance.SignUpdate("");
            for (int i = 0; i < 30; i++) Instantiate(enemyCardPrefab, enemyCardUi.GetChild(1));
            enemyHaveCards = enemyCardUi.GetChild(1).GetComponentsInChildren<CardBase>();
            MapInit();
        }

        public void StageUpdate() => StartCoroutine(UpdatingCoroutine());

        private IEnumerator UpdatingCoroutine()
        {
            DisplayManager.Instance.SignUpdate(stageList[0].mapType == MAP_TYPE.BATTLE ? "WHO'S NEXT?" : "CHOOSE ONE");
            yield return new WaitForSeconds(0.7f);
            if (stageList[0].mapType != MAP_TYPE.EVENT) SetItem();
        }
        
        public void StageClear()
        {
            SelectorPooling.Instance.ReturnPool(selectorPos.GetComponentsInChildren<SelectorItem>());
            ExplainManager.Instance.HideExplain();

            stageList.RemoveAt(0);

            if (stageList.Count == 0)
            {
                End.SetActive(true);
                return;
            }
            
            StageUpdate();
        }

        public void MapInit()
        {
            DisplayManager.Instance.SignUpdate("");

            End.SetActive(false);

            playerNameCard.Init(playerNormalSO);
            playerNameCard.transform.GetChild(0).transform.DOMoveY(-10, 0);
            enemyNameCard.gameObject.SetActive(false);

            stageList = new List<Stage>(stageSO.stageList);

            Release();
            _startBt.interactable = true;
            
            display.DOMoveY(displayPos.position.y, 0.7f).OnComplete(() => DisplayManager.Instance.SignUpdate("Own Card"));
        }

        public void GameStartBT()
        {
            _startBt.interactable = false;
            playerNameCard.transform.GetChild(0).gameObject.SetActive(true);
            playerNameCard.transform.GetChild(0).DOLocalMoveY(100, 1f).OnComplete(() => StageUpdate());
        }

        #region Battle
        private IEnumerator StartBattleCoroutine()
        {
            DisplayManager.Instance.SignUpdate("");
            battleUI.SetActive(true);
            display.DOMoveY(displayAnime.position.y, 0.8f);

            ArtifactManager.Instance.ArtifactsInit();

            yield return new WaitForSeconds(1f);

            ArtifactManager.Instance.OnEvent(EVENT_TYPE.STAGE_START, EVENT_TYPE.STAGE_START);
        }

        public void Release()
        {
            SoundManager.Instance.StopBGM();
            Karin.GameManager.Instance.ReleaseGame();
            battleUI.SetActive(false);
            ExplainManager.Instance.HideExplain();
        }

        public void BattleFin()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(enemyNameCard.transform.GetChild(0).DOMoveY(8, 1.5f).OnComplete(() =>
            {
                enemyNameCard.gameObject.SetActive(false);
                enemyNameCard.transform.GetChild(0).DOLocalMoveY(-100, 0);
            }));

            Release();

            if (playerNameCard.health <= 0)
            {
                seq.Append(display.DOMoveY(displayPos.position.y, 0.7f));
                seq.OnComplete(() => DisplayManager.Instance.DieSign());
            }
            else if (enemyNameCard.health <= 0)
            {
                int _value = (curSelectItem as Selector_Character).data.life - 5;

                seq.Append(display.DOMoveY(displayPos.position.y, 0.7f).OnStart(()=>
                    Damage(_value >= 5 ? _value : 5, Turn.Enemy, ATTACK_TYPE.HEAL, false)));
                seq.OnComplete(() => StageClear());
            }
        }

        public bool Damage(int _value, Turn _turn, ATTACK_TYPE _atkType = ATTACK_TYPE.ATTACK, bool cardEffect = true)
        {
            _value *= (_atkType == ATTACK_TYPE.ATTACK) ? -1 : 1;

            if (_turn == Turn.Enemy)
            {
                HealthEffect.Instance.HealthEvent(_value, playerNameCard, cardEffect);
                return playerNameCard.health - _value <= 0;
            }
            else if (_turn == Turn.Player)
            {
                HealthEffect.Instance.HealthEvent(_value, enemyNameCard, cardEffect);
                return enemyNameCard.health - _value <= 0;
            }
            return false;
        }
        #endregion
    }
}
