using UnityEngine;
using Shy;

public class HeartNecklace : ArtifactEffect
{
    //��Ʈ ī�带 7�� �� ������ 3�� Ĩ�� ����ϴ�.
    private int cnt;
    [SerializeField] private int value = 5;

    public override void Effect(Selector_Enemy _opponent)
    {
        if (ArtifactManager.Instance.currentUseCard.shape == Karin.BaseShapeType.Heart)
        {
            if(++cnt == 7)
            {
                //ü�� ȸ��
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
