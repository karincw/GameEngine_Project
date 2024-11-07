using UnityEngine;
using UnityEngine.EventSystems;

namespace Shy
{
    public class SelectorItem : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            StageManager.Instance.ChooseItem(gameObject);
        }
    }
}