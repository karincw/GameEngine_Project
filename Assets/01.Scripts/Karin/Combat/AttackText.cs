using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Karin
{

    public class AttackText : MonoBehaviour
    {
        [Header("Settings")]
        private TextMeshProUGUI _text;

        public int delayCnt;

        public int Count
        {
            get => _count;
            set
            {
                if (_count == value) return;

                SizeUpEffect();
                Fade(true);
                value = Mathf.Max(0, value);
                if (value > Count)
                {
                    value += delayCnt;
                    delayCnt = 0;
                    StartCoroutine(CountChangeCoroutine(1, _count, value));
                }
                else if (value < Count)
                {
                    StartCoroutine(CountChangeCoroutine(-1, _count, value));
                }

                _count = value;
            }
        }

        public static bool isAttackSticker(SpecialShapeType _type)
        {
            if (_type == SpecialShapeType.Sword2) return true;
            if (_type == SpecialShapeType.Sword3) return true;
            if (_type == SpecialShapeType.Sword5) return true;
            if (_type == SpecialShapeType.Sword7) return true;

            return false;
        }

        [SerializeField] private int _count;

        [Header("Fade")]
        [SerializeField] private float _fadeTime;
        [SerializeField, ColorUsage(showAlpha: true)] private Color _fadeOutColor;
        [SerializeField, ColorUsage(showAlpha: true)] private Color _fadeInColor;

        [Header("SizeUp")]
        [SerializeField] private float _sizeUpTime;
        [SerializeField] private Vector3 _targetSize;

        WaitForSeconds wait = new WaitForSeconds(.05f);

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            Count = 0;
        }

        private void Start()
        {
            _text.color = _fadeOutColor;
        }

        [ContextMenu("test")]
        private void Test()
        {
            Count = _count + 10;
        }

        public IEnumerator CountChangeCoroutine(int add, int now, int final)
        {
            Debug.Log("Count : " + Count + " / final : " + final);
            while (final != now)
            {
                now += add;
                _text.SetText(now.ToString());
                yield return wait;
            }
        }

        public void Fade(bool fadeIn)
        {
            if (fadeIn)
            {
                _text.DOColor(_fadeInColor, _fadeTime).SetEase(Ease.Linear);
            }
            else
            {
                _text.DOColor(_fadeOutColor, _fadeTime).SetEase(Ease.Linear);
            }
        }

        public void SizeUpEffect()
        {
            (transform as RectTransform).DOComplete();
            (transform as RectTransform).DOPunchScale(_targetSize, _sizeUpTime, 1000, 100).SetId(2);
        }
    }

}