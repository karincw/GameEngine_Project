using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public class Tester : MonoSingleton<Tester>
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
                HealthEffect.Instance.HealthEvent(-12, GameManager.Instance.enemyNameCard);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameManager.Instance.StageUpdate();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                DisplayManager.Instance.SignUpdate(Karin.Turn.Player);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                //Coin_Turn.Instance.CoinToss();
            }
        }
#endif
    }
}

