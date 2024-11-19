using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public EVENT_TYPE testType;

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("플레이어 턴 시작");
                ArtifactManager.Instance.OnEvent(testType);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("플레이어 턴 시작");
                DamageEffect.Instance.Damage(-12, StageManager.Instance.playerCard);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StageManager.Instance.StageUpdate();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                DisplaySign.Instance.SignUpdate(Karin.Turn.Player);
            }
        }
#endif
    }
}

