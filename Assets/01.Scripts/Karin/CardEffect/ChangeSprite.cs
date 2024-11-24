using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Karin
{

    public class ChangeSprite : MonoBehaviour
    {
        private CardBase _owner;
        public List<SelectShape> shapes;
        public Image _image;
        BaseShapeType originShape;

        public void Init(CardBase c)
        {
            _owner = c;
            foreach (var sel in shapes)
            {
                sel.Init(this);
            };

            shapes.ForEach(s => s.SetImge(false));
            _image.enabled = false;
        }

        public void SelectStart()
        {
            shapes.ForEach(s => s.SetImge(true));
            _image.enabled = true;
            TurnManager.Instance.turnChangeBtn.interactable = false;
        }

        public void SelectEnd()
        {
            shapes.ForEach(s => s.SetImge(false));
            _image.enabled = false;
            TurnManager.Instance.turnChangeBtn.interactable = true;
        }

        public void Selected(BaseShapeType shape)
        {
            if (_owner == null) return;

            originShape = _owner.cardData.shape;
            _owner.cardData.shape = shape;
            _owner.CardVisual.SetVisual(true);

            SelectEnd();
        }

        public void SetOriginShape()
        {
            _owner.cardData.shape = originShape;
        }
    }

}