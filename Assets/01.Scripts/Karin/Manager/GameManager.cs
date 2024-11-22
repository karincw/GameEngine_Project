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
            if (Input.GetKeyDown(KeyCode.F9))
            {
                TurnManager.Instance.ChangeTurn();
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                DebugCardView();
            }
        }

#endif

        [ContextMenu("GameStart")]
        public void GameStart()
        {
            Debug.Log("GameStart");
            cardPack.SetCards(PlayerCardHolder.myCards);
            cardPlace.CardSetting();
            PlayerCardHolder.StartSettings();
            EnemyCardHolder.StartSettings();
        }

        [ContextMenu("DebugCardView")]
        public void DebugCardView() 
        {
            FindObjectsOfType<CardVisual>().ToList().ForEach(c =>
            {
                c.SetVisual(true);
            });
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
                data.specialShape = (SpecialShapeType)s++;
                data.count = c;
                PlayerCardHolder.myCards.Add(data);
            }
        }

    }

}