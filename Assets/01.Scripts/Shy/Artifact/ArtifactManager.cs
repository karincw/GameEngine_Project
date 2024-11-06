using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public enum EVENT_TYPE
    {
        NONE = 0,
        TURN_START,
        TURN_END,
        GET_DAMAGE, //피해를 입을 때
        SET_DAMAGE, //데미지 스택을 쌓았을 때
        CAUSE_DAMAGE, //피해를 줬을 때
        STAGE_START,
        STAGE_END,
        USE_CARD, //카드를 냈을 때
        GET_CARD, //카드를 얻었을 때
    }

    public class ArtifactManager : MonoSingleton<ArtifactManager>
    {
        public List<Artifact> p_Atf;
        public List<Artifact> e_Atf;

        public Artifact obj;
        public Transform pos;

        public EVENT_TYPE testType;

        public void OnEvent(EVENT_TYPE _pType = EVENT_TYPE.NONE, EVENT_TYPE _eType = EVENT_TYPE.NONE)
        {
            if(_pType != EVENT_TYPE.NONE)
            {
                foreach (Artifact item in p_Atf)
                {
                    for (int i = 0; i < item.effects.Length; i++)
                        if (item.effects[i].eType == _pType) item.effects[i].Effect();
                }
            }

            if (_eType != EVENT_TYPE.NONE)
            {
                foreach (Artifact item in e_Atf)
                {
                    for (int i = 0; i < item.effects.Length; i++)
                        if (item.effects[i].eType == _eType)
                        {
                            item.effects[i].Effect();
                            break;
                        }
                }
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("플레이어 턴 시작");
                OnEvent(testType);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                Instantiate(obj, pos);
            }
        }
    }
}
