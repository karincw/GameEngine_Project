using UnityEngine;
using UnityEngine.EventSystems;

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
