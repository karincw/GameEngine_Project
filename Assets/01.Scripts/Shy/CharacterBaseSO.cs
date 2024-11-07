using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    [CreateAssetMenu(menuName = "SO/Shy/Character")]
    public class CharacterBaseSO : ScriptableObject
    {
        public string cName;
        public int life;
        //public List<CardBase> deck;
        public List<Artifact> artifacts;
    }
}
