using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Shy
{
    public class HealthEffect : MonoSingleton<HealthEffect>
    {
        [SerializeField] private Color minusColor;
        [SerializeField] private Color plusColor;
        [SerializeField] private TextMeshProUGUI damageTxt;
        [SerializeField] private ParticleSystem particle;

        [SerializeField] private Transform pStackPos;
        [SerializeField] private Transform eStackPos;

        public void HealthEvent(int _value, Selector_Character _target, bool cardEffect = true)
        {
            damageTxt.gameObject.SetActive(true);
            damageTxt.text = "  " + Mathf.Abs(_value).ToString();
            damageTxt.color = _value < 0 ? minusColor : plusColor;
            damageTxt.transform.localScale = Vector3.one;

            if (cardEffect)
                damageTxt.transform.position = _target.name.Contains("Player") ? pStackPos.position : eStackPos.position;
            else
                damageTxt.transform.position = Vector3.zero;

            Karin.GameManager.Instance.PlayerCardHolder.CardDrag(false);

            Sequence seq = DOTween.Sequence();
            seq.Append(damageTxt.transform.DOMove(_target.transform.GetChild(0).Find("Coin_Img").GetChild(0).position, 0.8f)
                .OnComplete(()=>
                {
                    particle.transform.position = damageTxt.transform.position;
                    if (_value < 0) particle.Play();
                        damageTxt.gameObject.SetActive(false);

                    StartCoroutine(HealthAnime(_value, _target, cardEffect));
                }));
            seq.Insert(0, damageTxt.transform.DOScale(0.5f, 0.5f));
        }

        private IEnumerator HealthAnime(int _value, Selector_Character _target, bool _turnChange)
        {
            for (int i = 0; i < Mathf.Abs(_value); i++)
            {
                _target.health += _value >= 0 ? 1 : -1;
                yield return new WaitForSeconds(0.01f);
            }

            if (_target.health <= 0)
            {
                yield return new WaitForSeconds(1.2f);
                GameManager.Instance.BattleFin();
            }
            else if (_turnChange)
            {
                if(_target.health > 0)
                    Coin.Instance.CoinToss(Karin.TurnManager.Instance.CurrentTurn, Karin.TurnManager.Instance.turnChangeBtn);
            }
        }
    }
}