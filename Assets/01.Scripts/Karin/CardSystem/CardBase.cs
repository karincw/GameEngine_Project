using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{

    public class CardBase : MonoBehaviour
    {
        [SerializeField] private CardBaseSO _cardData;

        public void Init()
        {

        }

        public bool CanUse(int c, BaseShapeType s)
        {
            if (_cardData == null) return false;

            if (_cardData.count.Equals(c) || ((int)_cardData.Shape & (int)s) > 0)
            {
                return true;
            }

            return false;

        }
    }

}
