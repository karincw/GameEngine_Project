using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Karin
{

    public class CardHolder : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private InputReaderSO _inputReader;
        [SerializeField] private Transform _cardSpawnTrm;
        [SerializeField] private GraphicRaycaster _graphicRaycaster;
        [SerializeField] private float _xPadding;
        public float layoutXDelta;

        [SerializeField] private CardVisual _selectCard;

        [SerializeField] private List<CardBase> cards = new List<CardBase>();
        [SerializeField] private List<float> layouts = new List<float>();

        [Header("Prefabs")]
        [SerializeField] private CardBase _cardPrefabs;

        private RectTransform _rectTrm;
        private float holderWidth;

        private void Awake()
        {
            _rectTrm = transform as RectTransform;
            holderWidth = _rectTrm.sizeDelta.x - _xPadding;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                AddCard();
            }
        }

        public CardDataSO testData;
        [ContextMenu("TestAdd")]
        public void AddCard()
        {
            CardBase cb = Instantiate(_cardPrefabs, _cardSpawnTrm);
            cb.Initialize(testData, this, _graphicRaycaster);
            cards.Insert(0, cb);
            AddLayout();
            SortingLayerOrder();
            ApplyLayoutWithTween(0.4f, 1);

            RectTransform rectTrm = cb.transform as RectTransform;

            float leftPos = -holderWidth / 2;
            float lengthDelta = holderWidth / (layouts.Count + 1);

            rectTrm.localPosition = new Vector3(leftPos - 300, 0, 0);
            rectTrm.DOLocalMoveX(leftPos + lengthDelta, 0.4f).SetEase(Ease.InExpo);
        }

        public void AddCard(CardDataSO data)
        {

        }

        public void UseCard()
        {

        }

        private void AddLayout()
        {
            layouts.Add(1000);
            SortingLayout();
        }
        public void SortingLayerOrder()
        {
            if (cards.Count < 0 || cards == null) return;

            int cardCount = cards.Count;
            int siblingIdx = cardCount + 1;
            for (int i = 0; i < cardCount; i++)
            {
                cards[i].transform.SetSiblingIndex(i);
                //cards[i].gameObject.name = $"Card idx[{i}] s[{siblingIdx}]";
            }
        }
        public void SortingLayout()
        {
            if (layouts.Count < 0 || layouts == null) return;
            int count = layouts.Count;

            float lengthDelta = holderWidth / (count + 1);
            float leftPos = -holderWidth / 2;
            float prev = leftPos;

            for (int i = 0; i < layouts.Count; i++)
            {
                layouts[i] = prev + lengthDelta;
                prev = layouts[i];
            }
        }
        public void ApplyLayout(int startIdx = 0)
        {
            int len = Mathf.Min(layouts.Count, cards.Count);
            Vector3 temp = Vector3.zero;
            temp.x += -holderWidth / 2;

            for (int i = startIdx; i < len; i++)
            {
                temp.x = layouts[i];
                (cards[i].transform as RectTransform).localPosition = temp;
            }
        }
        public void ApplyLayoutWithTween(float speed, int startIdx = 0)
        {
            int len = Mathf.Min(layouts.Count, cards.Count);

            for (int i = startIdx; i < len; i++)
            {
                float temp = layouts[i];
                (cards[i].transform as RectTransform).DOLocalMoveX(temp, speed);
            }
        }
    }

}