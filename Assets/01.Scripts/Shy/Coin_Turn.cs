using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

using Karin;
using UnityEngine.UI;

public class Coin_Turn : MonoSingleton<Coin_Turn>
{
    private TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void CoinToss(Turn _cur, Button _bt)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DORotate(new Vector3(179, 179, 179f), 0.4f).OnComplete(() => transform.rotation = Quaternion.Euler(Vector3.zero)));
        seq.Insert(0.2f, tmp.transform.DOLocalMove(Vector3.zero, 0)
            .OnComplete(() => tmp.text = (_cur != Turn.Player ? "E" : "Y")))
            .OnComplete(()=>
            {
                if (_cur == Turn.Enemy)
                {
                    GameManager.Instance.EnemyCardHolder.AutoRun();
                    GameManager.Instance.PlayerCardHolder.CardDrag(false);
                }
                else if (_cur == Turn.Player)
                {
                    _bt.interactable = true;
                    GameManager.Instance.PlayerCardHolder.CardDrag(true);
                }
            });
    }
}
