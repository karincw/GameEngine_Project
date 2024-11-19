using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Shy
{
    public class DisplaySign : MonoSingleton<DisplaySign>
    {
        [SerializeField] private Color baseColor;
        [SerializeField] private TextMeshProUGUI boardMes;
        [SerializeField] private float delay;
        [SerializeField] private int roopCnt;

        public void SignUpdate(Karin.Turn _turn)
        {
            SignUpdate(Karin.Turn.Player == _turn ? "Your Turn" : "Other Turn");
        }

        public void SignUpdate(string _mes)
        {
            boardMes.text = _mes;

            boardMes.color = Color.black;
            Color shineColor = baseColor + new Color(1, 1, 0);

            Sequence seq = DOTween.Sequence();
            
            for (int i = 0; i < roopCnt; i++)
            {
                seq.Append(boardMes.DOColor(baseColor, delay));
                seq.Append(boardMes.DOColor(shineColor, delay));
            }
        }
    }
}