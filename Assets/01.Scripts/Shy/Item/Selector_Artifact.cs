using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shy
{
    public class Selector_Artifact : SelectorItem
    {
        private ArtifactData data;

        public override void Init(Item_DataSO _base)
        {
            data = _base as ArtifactData;

            transform.GetChild(0).GetComponent<Image>().sprite = data.artifact.GetComponent<Image>().sprite;

            gameObject.SetActive(true);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            ExplainManager.Instance.ShowExplain(data, gameObject);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            GameManager.Instance.AddArtifact(data);
            GameManager.Instance.StageClear();
        }
    }
}
