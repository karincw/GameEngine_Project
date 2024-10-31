using Karin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardHolder : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private Transform _visualHolderTrm;
    [SerializeField] private GraphicRaycaster _graphicRaycaster;

    [SerializeField] private CardVisual _selectCard;

    [Header("Prefabs")]
    [SerializeField] private CardBase _cardPrefabs;
    [SerializeField] private GameObject _layoutSlotPrefabs;

    private RectTransform _rectTrm;
    private bool _isCross = false;
    private float offset => _rectTrm.sizeDelta.x / cards.Count;

    private List<CardBase> cards = new List<CardBase>();
    private List<GameObject> layouts = new List<GameObject>();

    private void Awake()
    {
        _rectTrm = transform as RectTransform;
    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

    private void Update()
    {

    }

    public void AddCard(CardDataSO data)
    {

    }

    public void UseCard()
    {

    }

}
