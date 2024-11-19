using TMPro;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Shape = Karin.SpecialShapeType;

namespace Shy
{
    public class ExplainManager : MonoSingleton<ExplainManager>
    {
        private RectTransform panel;
        private TextMeshProUGUI itemName;
        private TextMeshProUGUI itemExplain;

        [SerializeField] private SerializedDictionary<Shape, Item_DataSO> shapeToExplain;

        private void Awake()
        {
            panel = transform.GetChild(0).GetComponent<RectTransform>();
            itemName = panel.Find("ItemName").GetComponent<TextMeshProUGUI>();
            itemExplain = panel.Find("ItemExplain").GetComponent<TextMeshProUGUI>();

            HideExplain();
        }

        public void ShowExplain(Shape _data, GameObject _target)
        {
            if (_data == Shape.Diamond || _data == Shape.Heart || _data == Shape.Clover || _data == Shape.Spade) return;
            ShowExplain(shapeToExplain[_data], _target);
        }

        public void ShowExplain(Item_DataSO _data, GameObject _target)
        {
            panel.gameObject.SetActive(true);

            string explain = _data.itemExplain;
            Vector2 size = new Vector2(400, 135);

            int i;
            for (i = 0;  i < _data.itemExplain.Length / 17; i++)
                explain.Insert(17 * i, "\n");

            if (i > 3) size = new Vector2(400, 45 + i * 30);

            panel.sizeDelta = size;

            itemName.text = _data.itemName;
            itemExplain.text = explain;

            if(_target.TryGetComponent(out RectTransform _pos))
            {
                panel.position = _pos.position;
                panel.localPosition += new Vector3(_pos.rect.width / 2 + 210, 0) 
                    * (_pos.position.x <= 0 ? 1 : -1);
            }
        }

        public void HideExplain()
        {
            panel.gameObject.SetActive(false);
        }
    }
}