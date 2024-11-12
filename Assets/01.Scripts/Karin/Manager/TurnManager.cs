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

        public void ChangeTurn()
        {
            if (currentTurn == Turn.Player)
            {
                currentTurn = Turn.Enemy;
                GameManager.Instance.cardHolder.CardDrag(false);
            }
            else if (currentTurn == Turn.Enemy)
            {
                currentTurn = Turn.Player;
                GameManager.Instance.cardHolder.CardDrag(true);

            }
        }
        public void ChangeTurn(Turn who)
        {
            currentTurn = who;
            GameManager.Instance.cardHolder.CardDrag(who == Turn.Player);
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
        }
    }

}