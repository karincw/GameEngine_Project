using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public abstract class ArtifactEffect : MonoBehaviour
    {
        public EVENT_TYPE eType;
        [TextArea(), SerializeField] private string explain;

        public abstract void Effect();
    }
}