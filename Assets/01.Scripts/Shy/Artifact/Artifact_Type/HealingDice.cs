using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shy;

public class HealingDice : ArtifactEffect
{
    public override void Effect(Selector_Enemy _opponent)
    {
        transform.parent.GetComponentInParent<Selector_Enemy>().health += Random.Range(1, 7) * 2;
    }

    public override void Init(){}
}
