using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Shy
{
    public class DamageEffect : MonoSingleton<DamageEffect>
    {
        [SerializeField] private Color minusColor;
        [SerializeField] private Color plusColor;
        [SerializeField] private TextMeshProUGUI damageTxt;
        [SerializeField] private ParticleSystem particle;

        public void Damage(int _value, Selector_Enemy _target)
        {
            damageTxt.gameObject.SetActive(true);

            damageTxt.text = "   " + _value.ToString();
            damageTxt.color = _value < 0 ? minusColor : plusColor;

            damageTxt.transform.localScale = Vector3.one;
            damageTxt.transform.position = Vector3.zero;

            Sequence seq = DOTween.Sequence();

            seq.Append(damageTxt.transform.DOMove(_target.transform.GetChild(0).Find("Coin_Img").GetChild(0).position, 3f));
            seq.Insert(1.3f, damageTxt.transform.DOScale(0.5f, 1.5f)).OnComplete(
                () => {
                    particle.transform.position = damageTxt.transform.position;
                    if(_value < 0) particle.Play();
                    damageTxt.gameObject.SetActive(false);
                    StartCoroutine(HealthAnime(_value, _target));
                });
        }

        private IEnumerator HealthAnime(int _value, Selector_Enemy _target)
        {
            for (int i = 0; i < Mathf.Abs(_value); i++)
            {
                _target.health += _value >= 0 ? 1 : -1;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}