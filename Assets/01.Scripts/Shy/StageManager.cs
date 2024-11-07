using System.Collections.Generic;
using UnityEngine;


namespace Shy
{
    public class StageManager : MonoSingleton<StageManager>
    {
        [SerializeField] private Transform selectorPos;
        public GameObject curSelectObject;
        public GameObject lastChooseUI;
        [SerializeField] private NameCard nameCardPrefab;
        [SerializeField] private NameCard n;

        [SerializeField, Header("STAGE")] private StageListSO stageSO;
        [SerializeField] private List<Stage> nowMap;

        public void SetSelector()
        {
            selectorPos.gameObject.SetActive(true);

            int rand = (int)Random.Range(Mathf.Min(nowMap[0].spawnCnt.x, nowMap[0].spawnCnt.y), 
                Mathf.Max(nowMap[0].spawnCnt.x, nowMap[0].spawnCnt.y) + 1);

            //Pool
            while (selectorPos.childCount < rand)
                Instantiate(nameCardPrefab, selectorPos).gameObject.SetActive(false);

            List<CharacterBaseSO> cList = new List<CharacterBaseSO>();

            for (int i = 0; i < selectorPos.childCount; i++)
            {
                if(i < rand)
                {
                    CharacterBaseSO c = nowMap[0].spawnEnemy[Random.Range(0, nowMap[0].spawnEnemy.Count)];
                    selectorPos.GetChild(i).GetComponent<NameCard>().Init(c);
                    nowMap[0].spawnEnemy.Remove(c);
                    cList.Add(c);

                    selectorPos.GetChild(i).gameObject.SetActive(true);
                }
                else
                    selectorPos.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < cList.Count; i++)
                nowMap[0].spawnEnemy.Add(cList[i]);
        }

        public void ChooseItem(GameObject _obj)
        {
            curSelectObject = _obj;
            lastChooseUI.SetActive(true);
        }

        public void ChooseSelect()
        {
            lastChooseUI.SetActive(false);
            selectorPos.gameObject.SetActive(false);
            n.gameObject.SetActive(true);
            n.Init(curSelectObject.GetComponent<NameCard>().data);
        }

        public void ChooseCancel()
        {
            curSelectObject = null;
            lastChooseUI.SetActive(false);
        }

        private void StageUpdate()
        {
            Debug.Log("StageUpdate : " + nowMap[0]);
            DisplaySign.Instance.SignUpdate(nowMap[0].mapType == MAP_TYPE.BATTLE ? "WHO'S NEXT?" : "BONUS EVENT");

            SetSelector();
        }

        
        public void StageInit()
        {
            nowMap = new List<Stage>(stageSO.stageList);
        }

        private void Start()
        {
            n.gameObject.SetActive(false);
            StageInit();
        }

#if UNITY_EDITOR
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StageUpdate();
            }
        }
#endif
    }
}
