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
        [SerializeField] private TextMeshProUGUI[] numTmp;

        public override void Init(Item_DataSO _base)
        {
            data = _base as StickerData;
            CardManager _cm = CardManager.Instance;

            int _num = Random.Range(1, 14), _shape = Random.Range(0, 4);

            numTmp[0].text = _cm.GetCountText((CountType)_num);
            numTmp[1].text = _cm.GetCountText((CountType)_num);

            ChangeSprite(subIcon[0], (SpecialShapeType)_shape);
            ChangeSprite(subIcon[1], (SpecialShapeType)_shape);
            ChangeSprite(image, data.shape);

            cardData = new CardDataSO(CardType.Gold, (CountType)_num, (BaseShapeType)_shape, data.shape);
            gameObject.SetActive(true);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            Karin.GameManager.Instance.PlayerCardHolder.ChangeSpecialType(cardData);
            GameManager.Instance.StageClear();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            ExplainManager.Instance.ShowExplain(data, gameObject);
        }

        private void ChangeSprite(Image _img, SpecialShapeType _shape)
        {
            _img.sprite = CardManager.Instance.ShapeToSpriteDictionary[_shape];
            
            if (CardManager.Instance.ShapeToColorDictionary.ContainsKey(_shape))
                img.color = CardManager.Instance.ShapeToColorDictionary[_shape];
            else
                img.color = Color.white;
        }
    }
}
