using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{

    public class TurnManager : MonoSingleton<TurnManager>
    {
        public Turn currentTurn;

        [SerializeField] private AttackText _playerText;
        [SerializeField] private AttackText _enemyText;

        public bool useCard;

        public event Action<Turn> TurnChangedEvent;
        public event Action<Turn> OnAttackEvent;
        public event Action<Turn> OnDefenceEvent;

        public void ChangeTurn()
        {
            if (!useCard)
            {
                if (currentTurn == Turn.Player)
                {
                    GameManager.Instance.playerCardHolder.AddCard();
                }
                else if (currentTurn == Turn.Enemy)
                {
                    GameManager.Instance.enemyCardHolder.AddCard();
                }
            }

            if (currentTurn == Turn.Player)
            {
                currentTurn = Turn.Enemy;
                GameManager.Instance.playerCardHolder.CardDrag(false);
            }
            else if (currentTurn == Turn.Enemy)
            {
                currentTurn = Turn.Player;
                GameManager.Instance.playerCardHolder.CardDrag(true);
            }
            TurnChangedEvent?.Invoke(currentTurn);
            useCard = false;
        }

        public void ChangeTurn(Turn who)
        {
            currentTurn = who;
            GameManager.Instance.playerCardHolder.CardDrag(who == Turn.Player);
            TurnChangedEvent?.Invoke(currentTurn);
            useCard = false;
        }

        public void Attack(int damage)
        {
            if (currentTurn == Turn.Player)
            {
                _enemyText.Count += damage;
                _playerText.Count = 0;
                _playerText.Fade(false);
            }
            else if (currentTurn == Turn.Enemy)
            {
                _playerText.Count += damage;
                _enemyText.Count = 0;
                _enemyText.Fade(false);
            }
            OnAttackEvent?.Invoke(currentTurn);
        }

        public void Defence(int defence)
        {
            if (defence == -1)
                defence = _enemyText.Count;

            if (currentTurn == Turn.Player)
            {
                _enemyText.Count -= defence;
                _playerText.Count = 0;
                _playerText.Fade(false);
            }
            else if (currentTurn == Turn.Enemy)
            {
                _playerText.Count -= defence;
                _enemyText.Count = 0;
                _enemyText.Fade(false);
            }
            OnDefenceEvent?.Invoke(currentTurn);
        }
    }

}