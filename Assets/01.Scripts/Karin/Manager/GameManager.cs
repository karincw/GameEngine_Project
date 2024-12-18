using Shy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Karin
{

    public class GameManager : MonoSingleton<GameManager>
    {

        [HideInInspector] public CardPlace cardPlace;
        public CardHolder PlayerCardHolder;
        public EnemyCardHolder EnemyCardHolder;
        [HideInInspector] public CardPack cardPack;

        [SerializeField] private CardDataSO baseCardData;

        private void Awake()
        {
            cardPlace = FindObjectOfType<CardPlace>();
            cardPack = FindObjectOfType<CardPack>();
        }

        private void Start()
        {
            StartSettings();
        }

#if UNITY_EDITOR

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                DebugCardView();
            }
        }

        [ContextMenu("DebugCardView")]
        public void DebugCardView()
        {
            FindObjectsOfType<CardVisual>().ToList().ForEach(c =>
            {
                c.SetVisual(true);
            });
        }

#endif

        [ContextMenu("GameStart")]
        public void GameStart()
        {
            //ReleaseGame();
            Debug.Log("GameStart");
            cardPack.SetCards(PlayerCardHolder.myCards);
            cardPack.SetCards(StageManager.Instance.enemyNameCard.cardDataSoList);
            cardPlace.CardSetting();
            PlayerCardHolder.StartSettings(null);
            EnemyCardHolder.StartSettings();
        }

        public void ReleaseGame()
        {
            Debug.Log("ReleaseGame");
            cardPlace.Release();
            EnemyCardHolder.Release();
            PlayerCardHolder.Release();
            cardPack.Release();
            TurnManager.Instance.ChangeTurn(Turn.Player);
            Coin_Turn.Instance.CoinToss(Turn.Player, TurnManager.Instance.turnChangeBtn);
            TurnManager.Instance.ReleaseTexts();
        }

        public void StartSettings()
        {
            int delta = 5;
            CountType c = CountType.ACE;
            BaseShapeType s = 0;
            for (int i = 1; i < 53; i++)
            {
                CardDataSO data = Instantiate(baseCardData);
                if (i >= delta)
                {
                    c++;
                    delta += 4;
                }
                if ((int)s == 4)
                {
                    s = BaseShapeType.Diamond;
                }
                data.shape = s;
                data.count = c;
                data.specialShape = (SpecialShapeType)s++;

                if (data.count == CountType.Three)
                    data.specialShape = SpecialShapeType.Shield;
                if (data.count == CountType.ACE)
                    data.specialShape = SpecialShapeType.Sword3;
                if (data.count == CountType.Two)
                    data.specialShape = SpecialShapeType.Sword2;
                if (data.count == CountType.Seven)
                    data.specialShape = SpecialShapeType.ChangeShape;
                if (data.count == CountType.King)
                    data.specialShape = SpecialShapeType.King;

                PlayerCardHolder.myCards.Add(data);
            }
        }

    }

}