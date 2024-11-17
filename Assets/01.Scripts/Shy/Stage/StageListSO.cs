using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public enum MAP_TYPE
    {
        BATTLE = 0,
        EVENT,
        REWARD
    }

    [CreateAssetMenu(menuName = "SO/Shy/Stage")]
    public class StageListSO : ScriptableObject
    {
        public List<Stage> stageList;
    }

    [System.Serializable]public class Stage
    {
        public MAP_TYPE mapType;
        public List<Item_DataSO> spawnItem;
        [Tooltip("�ּ�, �ִ� ���� ��� ����")] public Vector2 spawnCnt;
    }
}
