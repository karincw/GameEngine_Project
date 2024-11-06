using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public class Artifact : MonoBehaviour
    {
        internal ArtifactEffect[] effects;

        private void Start()
        {
            Init();    
        }

        public void Init()
        {
            effects = GetComponents<ArtifactEffect>();
        }
    }
}
