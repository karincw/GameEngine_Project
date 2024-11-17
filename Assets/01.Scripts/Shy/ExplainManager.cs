using TMPro;
using UnityEngine;

namespace Shy
{
    public class ExplainManager : MonoSingleton<ExplainManager>
    {

        private Transform panel;
        private TextMeshProUGUI itemName;
        private TextMeshProUGUI itemExplain;

        private void Awake()
        {
            panel = transform.GetChild(0);
            itemName = panel.Find("ItemName").GetComponent<TextMeshProUGUI>();
            itemExplain = panel.Find("ItemExplain").GetComponent<TextMeshProUGUI>();

            HideExplain();
        }

        public void ShowExplain(Item_DataSO _data, GameObject _target)
        {
            panel.gameObject.SetActive(true);

            itemName.text = _data.itemName;
            itemExplain.text = _data.itemExplain;

            if(_target.TryGetComponent(out RectTransform _pos))
            {
                panel.position = _pos.position;
                panel.localPosition += new Vector3(_pos.rect.width / 2 + 210, 0);
            }
        }

        public void HideExplain()
        {
            panel.gameObject.SetActive(false);
        }
    }
}