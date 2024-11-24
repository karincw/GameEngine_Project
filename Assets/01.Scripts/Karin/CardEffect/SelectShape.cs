using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Karin
{

    public class SelectShape : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private BaseShapeType ShapeType;

        [Header("Scale-Settings")]
        [SerializeField] private float _scaleDuration = 0.2f;
        [SerializeField] private float _scaleExpandValue = 1.1f;

        private ChangeSprite _changeSprite;
        private Image _image;

        public void Init(ChangeSprite c)
        {
            _changeSprite = c;
            _image = GetComponent<Image>();
        }

        public void SetImge(bool visible)
        {
            _image.enabled = visible;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _changeSprite.Selected(ShapeType);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one * _scaleExpandValue, _scaleDuration).SetEase(Ease.Linear);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, _scaleDuration).SetEase(Ease.Linear);
        }
    }

}