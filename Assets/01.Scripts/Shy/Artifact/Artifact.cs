using UnityEngine;
using UnityEngine.EventSystems;

namespace Shy
{
    public class Artifact : MonoBehaviour, IPointerEnterHandler
    {
        internal ArtifactEffect[] effects;
        internal ArtifactData data;

        public void Init(ArtifactData _data)
        {
            data = _data;
            effects = GetComponents<ArtifactEffect>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ExplainManager.Instance.ShowExplain(data, gameObject);
        }
    }
}
