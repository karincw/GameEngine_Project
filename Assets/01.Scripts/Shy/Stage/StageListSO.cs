using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public enum MAP_TYPE
    {
        BATTLE = 0,
        EVENT
    }

    [CreateAssetMenu(menuName = "SO/Shy/Stage")]
    public class StageListSO : ScriptableObject
    {
        public List<Stage> stageList;
    }

    [System.Serializable]public class Stage
    {
        public MAP_TYPE mapType;
        public List<CharacterBaseSO> spawnEnemy;
        [Tooltip("최소, 최대 순서 상관 없음")] public Vector2 spawnCnt;
    }
}
