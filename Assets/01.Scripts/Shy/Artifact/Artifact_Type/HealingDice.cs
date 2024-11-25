using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shy;

public class HealingDice : ArtifactEffect
{
    public override void Effect(Selector_Enemy _opponent)
    {
        DamageEffect.Instance.Damage(Random.Range(1, 7) * 2, transform.parent.GetComponentInParent<Selector_Enemy>(), false);
    }

    public override void Init(){}
}
