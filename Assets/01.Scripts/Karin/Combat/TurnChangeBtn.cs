using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{

    public class TurnChangeBtn : MonoBehaviour
    {
        public void TurnChange()
        {
            TurnManager.Instance.ChangeTurn();
        }
    }

}