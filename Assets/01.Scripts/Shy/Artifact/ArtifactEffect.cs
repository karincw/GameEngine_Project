using UnityEngine;

namespace Shy
{
    public abstract class ArtifactEffect : MonoBehaviour
    {
        public EVENT_TYPE eType;

        public abstract void Effect(Selector_Character _opponent);

        public virtual void Init()
        { 
        }
    }
}
