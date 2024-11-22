using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Karin;
using UnityEngine.EventSystems;

namespace Shy
{
    public class Selector_Sticker : SelectorItem
    {
        private StickerData data;
        private CardDataSO cardData;
        [SerializeField] private Image image;
        [SerializeField] private Image[] subIcon;
        [SerializeField] private TextMeshProUGUI[] num;

        private int rand, shape;

        public override void Init(Item_DataSO _base)
        {
            data = _base as StickerData;
            CardManager cm = CardManager.Instance;

            rand = Random.Range(1, 14);
            shape = Random.Range(0, 4);

            num[0].text = cm.GetCountText((CountType)rand);
            num[1].text = cm.GetCountText((CountType)rand);

            ChangeSprite(subIcon[0], (SpecialShapeType)shape);
            ChangeSprite(subIcon[1], (SpecialShapeType)shape);
            ChangeSprite(image, data.shape);

            cardData = new CardDataSO(CardType.Gold, (CountType)rand, (BaseShapeType)shape, data.shape);
            gameObject.SetActive(true);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            Karin.GameManager.Instance.PlayerCardHolder.ChangeSpecialType(cardData);
            StageManager.Instance.StageClear();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            ExplainManager.Instance.ShowExplain(data, gameObject);
        }

        private void ChangeSprite(Image _img, SpecialShapeType _shape)
        {
            _img.sprite = CardManager.Instance.ShapeToSpriteDictionary[_shape];
            _img.color = CardManager.Instance.ShapeToColorDictionary[_shape];
        }
    }
}