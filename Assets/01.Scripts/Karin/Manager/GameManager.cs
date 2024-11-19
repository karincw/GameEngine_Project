using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{

    public class GameManager : MonoSingleton<GameManager>
    {

        [HideInInspector] public CardPlace cardPlace;
        [HideInInspector] public CardHolder cardHolder;
        [HideInInspector] public CardPack cardPack;

        private void Start()
        {
            cardPlace = FindObjectOfType<CardPlace>();
            cardHolder = FindObjectOfType<CardHolder>();
            cardPack = FindObjectOfType<CardPack>(); 
        }

#if UNITY_EDITOR

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                GameStart();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                TurnManager.lnstance.ChangeTurn();
            }
        }

#endif

        [ContextMenu("GameStart")]
        public void GameStart()
        {
            cardPlace.CardSetting();
            cardHolder.StartSettings();
        }
    }

}