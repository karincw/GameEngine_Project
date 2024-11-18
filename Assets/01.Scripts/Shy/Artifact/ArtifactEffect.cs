using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public abstract class ArtifactEffect : MonoBehaviour
    {
        public EVENT_TYPE eType;

        public abstract void Effect();

        public abstract void Init();
    }
}
