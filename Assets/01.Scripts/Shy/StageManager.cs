using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Shy
{
    public class StageManager : MonoSingleton<StageManager>
    {
        [SerializeField] private StageListSO stageSO;
        [SerializeField] private List<MAP_TYPE> nowMap;

        public void StageUpdate()
        {
            Debug.Log("StageUpdate : " + nowMap[0]);
            DisplaySign.Instance.SignUpdate(nowMap[0] == MAP_TYPE.BATTLE ? "Who's Next" : "s");
            nowMap.RemoveAt(0);
        }

        public void StageInit()
        {
            nowMap = new List<MAP_TYPE>(stageSO.stageList);
        }

        private void Start()
        {
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
