using DG.Tweening;
using Karin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHolder : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private Transform _cardSpawnTrm;
    [SerializeField] private GraphicRaycaster _graphicRaycaster;
    [SerializeField] private float _xPadding;

    [SerializeField] private CardVisual _selectCard;

    [SerializeField] private List<CardBase> cards = new List<CardBase>();
    [SerializeField] private List<float> layouts = new List<float>();

    [Header("Prefabs")]
    [SerializeField] private CardBase _cardPrefabs;

    private RectTransform _rectTrm;
    private bool _isCross = false;
    private float holderWidth;


    private void Awake()
    {
        _rectTrm = transform as RectTransform;
        holderWidth = _rectTrm.sizeDelta.x - _xPadding;
    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

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
        cb.Initialize(testData);
        cards.Insert(0, cb);
        AddLayout();
        ApplyLayoutWithTween(0.4f, 1);

        RectTransform rectTrm = cb.transform as RectTransform;

        float leftPos = -holderWidth / 2;
        float lengthDelta = holderWidth / (layouts.Count + 1);

        rectTrm.localPosition = new Vector3(leftPos - 300, 0, 0);
        rectTrm.DOLocalMoveX(leftPos + lengthDelta, 0.4f).SetEase(Ease.InExpo);
    }

    public void AddCard(CardDataSO data)
    {
        CardBase cb = Instantiate(_cardPrefabs, _cardSpawnTrm);
        cb.Initialize(data);
        cards.Add(cb);
        AddLayout();
        ApplyLayout();
    }

    public void UseCard()
    {

    }

    private void AddLayout()
    {
        layouts.Add(1000);
        SortingLayout();
    }
    private void SortingLayout()
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
    private void ApplyLayout(int startIdx = 0)
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
    private void ApplyLayoutWithTween(float speed, int startIdx = 0)
    {
        int len = Mathf.Min(layouts.Count, cards.Count);

        for (int i = startIdx; i < len; i++)
        {
            float temp = layouts[i];
            (cards[i].transform as RectTransform).DOLocalMoveX(temp, speed);
        }
    }
}
