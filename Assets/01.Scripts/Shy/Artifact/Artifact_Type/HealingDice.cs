using UnityEngine;
using Shy;

public class HealingDice : ArtifactEffect
{
    private Selector_Character user;

    public override void Effect(Selector_Character _opponent)
    {
        HealthEffect.Instance.HealthEvent(Random.Range(2, 13), user, false);
    }

    public override void Init()
    {
        user = transform.parent.GetComponentInParent<Selector_Character>();
    }
}
