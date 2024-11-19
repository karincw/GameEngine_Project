using UnityEngine;
using UnityEngine.EventSystems;

namespace Shy
{
    public enum ITEM_TYPE
    {
        NONE = 0,
        ENEMY,
        CARD,
        ARTIFACT,
        END
    }

    public abstract class SelectorItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public ITEM_TYPE iType;
        public abstract void Init(Item_DataSO _base);

        public abstract void OnPointerDown(PointerEventData eventData);

        public virtual void OnPointerEnter(PointerEventData eventData)
        { 
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            ExplainManager.lnstance.HideExplain();
        }
    }
}