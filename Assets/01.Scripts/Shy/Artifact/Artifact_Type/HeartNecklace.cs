using UnityEngine;
using Shy;

public class HeartNecklace : ArtifactEffect
{
    //하트 카드를 7장 낼 때마다 3의 칩을 얻습니다.
    private int cnt;
    [SerializeField] private int value = 5;

    public override void Effect(Selector_Enemy _opponent)
    {
        if (ArtifactManager.Instance.currentUseCard.shape == Karin.BaseShapeType.Heart)
        {
            if(++cnt == 7)
            {
                //체력 회복
                transform.parent.GetComponentInParent<Selector_Enemy>().health += value;
                cnt = 0;
            }
        }

    }

    public override void Init()
    {
        cnt = 0;
    }
}
