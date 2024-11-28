using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        public Button turnChangeBtn;

        public bool useCard;
        public AttackInfo hitInfo;
        private bool attack;
        public bool firstUse;

        public event Action<Turn> TurnChangedEvent;
        public event Action<Turn> OnAttackEvent;
        public event Action<Turn> OnDefenceEvent;
        public event Action<Turn> OnGiveEvent;
        public event Action<Turn> OnLensEvent;
        public event Action<Turn> OnReflectEvent;

        public void ReleaseTexts()
        {
            _playerText.Count = 0;
            _playerText.Fade(false);
            _enemyText.Count = 0;
            _enemyText.Fade(false);
        }

        public void ChangeTurn()
        {
            firstUse = false;

            turnChangeBtn.interactable = false;

            if (!useCard) // useCard == false
            {
                if (currentTurn == Turn.Player)
                {
                    GameManager.Instance.PlayerCardHolder.AddCard();
                }
                else if (currentTurn == Turn.Enemy)
                {
                    GameManager.Instance.EnemyCardHolder.AddCard();
                }
            }

            if (hitInfo.hit && currentTurn != hitInfo.who)
            {
                Debug.Log($"{currentTurn} <- hit / damage:{GetHitText().Count}");
                AttackText at = GetHitText();
                attack = true;
                if (Shy.StageManager.Instance.Damage(at.Count, currentTurn)) return;
                at.Count = 0;
                at.Fade(false);
                hitInfo.hit = false;
                hitInfo.nowhit = false;
            }

            if(attack == false)
            {
                Coin_Turn.Instance.CoinToss(currentTurn, turnChangeBtn);
                currentTurn = currentTurn == Turn.Player ? Turn.Enemy : Turn.Player;
            }
            

            useCard = false;
            attack = false;
            TurnChangedEvent?.Invoke(currentTurn);

        }

        private AttackText GetHitText()
        {
            return currentTurn != Turn.Player ? _enemyText : _playerText;
        }

        public void ChangeTurn(Turn who)
        {
            currentTurn = who;
            GameManager.Instance.PlayerCardHolder.CardDrag(who == Turn.Player);
            TurnChangedEvent?.Invoke(currentTurn);
            useCard = false;
            firstUse = false;
        }

        public void Attack(int damage)
        {
            if (damage <= 0) return;

            var nowDamage = Mathf.Max(_enemyText.Count, _playerText.Count);

            if (currentTurn == Turn.Player)
            {
                _enemyText.Count += nowDamage + damage;
                _playerText.Count = 0;
                _playerText.Fade(false);
            }
            else if (currentTurn == Turn.Enemy)
            {
                _playerText.Count += nowDamage + damage;
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
            if (defence == -1)
            {
                defence = Mathf.Max(_enemyText.Count, _playerText.Count);
            }

            if (currentTurn == Turn.Player)
            {
                _playerText.Count -= defence;
                _playerText.Fade(false);
                if (_playerText.Count <= 0)
                {
                    hitInfo.hit = false;
                    hitInfo.nowhit = false;
                }
            }
            else if (currentTurn == Turn.Enemy)
            {
                _enemyText.Count -= defence;
                _enemyText.Fade(false);
                if (_enemyText.Count <= 0)
                {
                    hitInfo.hit = false;
                    hitInfo.nowhit = false;
                }
            }


            OnDefenceEvent?.Invoke(currentTurn);
        }

        public void GiveCard(int count)
        {
            if (currentTurn == Turn.Player)
            {
                for (int i = 0; i < count; i++)
                    GameManager.Instance.EnemyCardHolder.AddCard();
            }
            else if (currentTurn == Turn.Enemy)
            {
                for (int i = 0; i < count; i++)
                    GameManager.Instance.PlayerCardHolder.AddCard();
            }
            OnGiveEvent?.Invoke(currentTurn);
        }

        public void Lens(int count)
        {
            if (currentTurn == Turn.Player)
            {
                for (int i = 0; i < count; i++)
                    GameManager.Instance.EnemyCardHolder.ViewCard();
            }
            else if (currentTurn == Turn.Enemy)
            {

            }
            OnLensEvent?.Invoke(currentTurn);
        }

        public void Reflect()
        {
            var nowDamage = Mathf.Max(_enemyText.Count, _playerText.Count);

            if (currentTurn == Turn.Player)
            {
                _playerText.Count -= nowDamage;
                _enemyText.Count += nowDamage;
                _playerText.Fade(false);

                if (_playerText.Count <= 0)
                {
                    hitInfo.nowhit = false;
                }
            }
            else if (currentTurn == Turn.Enemy)
            {
                _enemyText.Count -= nowDamage;
                _playerText.Count += nowDamage;
                _enemyText.Fade(false);

                if (_enemyText.Count <= 0)
                {
                    hitInfo.nowhit = false;
                }
            }


            OnReflectEvent?.Invoke(currentTurn);
        }

        public void Dice(float per = 0.5f)
        {
            if (Random.value <= per)
            {
                Defence(-1);
            }
            else
            {
                if (currentTurn == Turn.Player)
                {
                    _playerText.Count *= 2;
                }
                else if (currentTurn == Turn.Enemy)
                {
                    _enemyText.Count *= 2;
                }
            }
        }
    }

}