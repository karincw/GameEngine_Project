using System;
using UnityEngine;
using UnityEngine.UI;

namespace Karin
{
    public struct AttackInfo
    {
        public bool hit;
        public bool nowhit;
        public Turn who;
    }

    public class TurnManager : MonoSingleton<TurnManager>
    {
        public Turn currentTurn;

        [SerializeField] private AttackText _playerText;
        [SerializeField] private AttackText _enemyText;
        [SerializeField] private Button _turnChangeBtn;

        public bool useCard;
        public AttackInfo hitInfo;

        public event Action<Turn> TurnChangedEvent;
        public event Action<Turn> OnAttackEvent;
        public event Action<Turn> OnDefenceEvent;

        public void ChangeTurn()
        {
            if (!useCard) // useCard == false
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

            if (hitInfo.hit && currentTurn != hitInfo.who)
            {
                Debug.Log($"{currentTurn} <- hit / damage:{GetHitText().Count}");
                AttackText at = GetHitText();
                Shy.StageManager.Instance.Damage(at.Count, currentTurn);
                at.Count = 0;
                at.Fade(false);
                hitInfo.hit = false;
                hitInfo.nowhit = false;
            }

            if (currentTurn == Turn.Player)
            {
                currentTurn = Turn.Enemy;
                _turnChangeBtn.interactable = false;
                GameManager.Instance.enemyCardHolder.AutoRun();
                GameManager.Instance.playerCardHolder.CardDrag(false);
            }
            else if (currentTurn == Turn.Enemy)
            {
                currentTurn = Turn.Player;
                _turnChangeBtn.interactable = true;
                GameManager.Instance.playerCardHolder.CardDrag(true);
            }

            useCard = false;
            TurnChangedEvent?.Invoke(currentTurn);

        }

        private AttackText GetHitText()
        {
            return currentTurn != Turn.Player ? _enemyText : _playerText;
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
            if (damage <= 0) return;

            if (currentTurn == Turn.Player)
            {
                _enemyText.Count += _playerText.Count + damage;
                _playerText.Count = 0;
                _playerText.Fade(false);
            }
            else if (currentTurn == Turn.Enemy)
            {
                _playerText.Count += _enemyText.Count + damage;
                _enemyText.Count = 0;
                _enemyText.Fade(false);
            }
            hitInfo.hit = true;
            hitInfo.nowhit = true;
            hitInfo.who = currentTurn;
            OnAttackEvent?.Invoke(currentTurn);
        }

        public void Defence(int defence)
        {
            if (defence <= 0) return;

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