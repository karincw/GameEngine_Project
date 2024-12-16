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
        public Turn CurrentTurn { get => _currentTurn; }
        private Turn _currentTurn;

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
            Debug.Log("Turn Change!!!" + " / curTurn = " + _currentTurn.ToString());
            if (_currentTurn == Turn.Player) turnChangeBtn.interactable = false;
            firstUse = false;
            turnChangeBtn.interactable = false;


            if (!useCard) // useCard == false
            {
                if (_currentTurn == Turn.Player)
                {
                    GameManager.Instance.PlayerCardHolder.AddCard();
                }
                else if (_currentTurn == Turn.Enemy)
                {
                    GameManager.Instance.EnemyCardHolder.AddCard();
                }
            }

            _currentTurn = _currentTurn == Turn.Player ? Turn.Enemy : Turn.Player;

            if (hitInfo.hit && _currentTurn == hitInfo.who)
            {
                Debug.Log($"{_currentTurn} <- hit / damage:{GetHitText().Count}");
                AttackText at = GetHitText();
                attack = true;
                
                if (Shy.StageManager.Instance.Damage(at.Count, _currentTurn)) return;

                at.Count = 0;
                at.Fade(false);
                hitInfo.hit = false;
                hitInfo.nowhit = false;
            }
            else
            {
                Coin_Turn.Instance.CoinToss(_currentTurn, turnChangeBtn);
            }
            
            useCard = false;
            attack = false;
            TurnChangedEvent?.Invoke(_currentTurn);

        }

        private AttackText GetHitText()
        {
            return _currentTurn == Turn.Player ? _enemyText : _playerText;
        }

        public void ChangeTurn(Turn who)
        {
            Debug.Log("Turn Change!!!");
            _currentTurn = who;
            GameManager.Instance.PlayerCardHolder.CardDrag(who == Turn.Player);
            TurnChangedEvent?.Invoke(_currentTurn);
            useCard = false;
            firstUse = false;
        }

        public void Attack(int damage, bool delay = false)
        {
            Debug.Log("Attack Him!!! " + damage);
            if (damage <= 0) return;

            var nowDamage = Mathf.Max(_enemyText.Count, _playerText.Count);

            if (_currentTurn == Turn.Player)
            {
                if(delay)
                {
                    _enemyText.delayCnt = nowDamage + damage;
                }
                else
                {
                    _enemyText.Count = nowDamage + damage;
                }

                _playerText.Count = 0;
                _playerText.Fade(false);
            }
            else if (_currentTurn == Turn.Enemy)
            {
                if (delay)
                {
                    _playerText.delayCnt = nowDamage + damage;
                }
                else
                {
                    _playerText.Count = nowDamage + damage;
                }
                
                _enemyText.Count = 0;
                _enemyText.Fade(false);
            }
            hitInfo.hit = true;
            hitInfo.nowhit = true;
            hitInfo.who = _currentTurn;
            OnAttackEvent?.Invoke(_currentTurn);
        }

        public void Defence(int defence)
        {
            if (defence == -1)
            {
                defence = Mathf.Max(_enemyText.Count, _playerText.Count);
            }

            if (_currentTurn == Turn.Player)
            {
                _playerText.Count -= defence;
                _playerText.Fade(false);
                if (_playerText.Count <= 0)
                {
                    hitInfo.hit = false;
                    hitInfo.nowhit = false;
                }
            }
            else if (_currentTurn == Turn.Enemy)
            {
                _enemyText.Count -= defence;
                _enemyText.Fade(false);
                if (_enemyText.Count <= 0)
                {
                    hitInfo.hit = false;
                    hitInfo.nowhit = false;
                }
            }


            OnDefenceEvent?.Invoke(_currentTurn);
        }

        public void GiveCard(int count)
        {
            if (_currentTurn == Turn.Player)
            {
                for (int i = 0; i < count; i++)
                    GameManager.Instance.EnemyCardHolder.AddCard();
            }
            else if (_currentTurn == Turn.Enemy)
            {
                for (int i = 0; i < count; i++)
                    GameManager.Instance.PlayerCardHolder.AddCard();
            }
            OnGiveEvent?.Invoke(_currentTurn);
        }

        public void Lens(int count)
        {
            if (_currentTurn == Turn.Player)
            {
                for (int i = 0; i < count; i++)
                    GameManager.Instance.EnemyCardHolder.ViewCard();
            }
            else if (_currentTurn == Turn.Enemy)
            {

            }
            OnLensEvent?.Invoke(_currentTurn);
        }

        public void Reflect()
        {
            var nowDamage = Mathf.Max(_enemyText.Count, _playerText.Count);

            if (_currentTurn == Turn.Player)
            {
                _playerText.Count -= nowDamage;
                _enemyText.Count += nowDamage;
                _playerText.Fade(false);

                if (_playerText.Count <= 0)
                {
                    hitInfo.nowhit = false;
                }
            }
            else if (_currentTurn == Turn.Enemy)
            {
                _enemyText.Count -= nowDamage;
                _playerText.Count += nowDamage;
                _enemyText.Fade(false);

                if (_enemyText.Count <= 0)
                {
                    hitInfo.nowhit = false;
                }
            }


            OnReflectEvent?.Invoke(_currentTurn);
        }

        public void Dice(float per = 0.5f)
        {
            if (Random.value <= per)
            {
                Defence(-1);
            }
            else
            {
                if (_currentTurn == Turn.Player)
                {
                    _playerText.Count *= 2;
                }
                else if (_currentTurn == Turn.Enemy)
                {
                    _enemyText.Count *= 2;
                }
            }
        }
    }

}