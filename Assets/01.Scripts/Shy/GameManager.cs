using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public NameCard playerCard;
        public NameCard enemyCard;

        public Artifact obj;
        public EVENT_TYPE testType;
        public Transform pos;


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
                Artifact art = Instantiate(obj, pos);
                playerCard.artifacts.Add(art);
            }
        }
#endif
    }
}

