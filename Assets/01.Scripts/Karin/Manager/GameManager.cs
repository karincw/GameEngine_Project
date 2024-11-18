using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{

    public class GameManager : MonoSingleton<GameManager>
    {

        [HideInInspector] public CardPlace cardPlace;
        public CardHolder playerCardHolder;
        public CardHolder enemyCardHolder;
        [HideInInspector] public CardPack cardPack;

        private void Start()
        {
            cardPlace = FindObjectOfType<CardPlace>();
            cardPack = FindObjectOfType<CardPack>();
        }

#if UNITY_EDITOR

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameStart();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                TurnManager.Instance.ChangeTurn();
            }
        }

#endif

        [ContextMenu("GameStart")]
        public void GameStart()
        {
            cardPlace.CardSetting();
            playerCardHolder.StartSettings();
        }

        public void StartSettings()
        {

        }

    }

}