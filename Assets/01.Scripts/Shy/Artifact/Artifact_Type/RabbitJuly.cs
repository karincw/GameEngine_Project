using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public class RabbitJuly : ArtifactEffect
    {
        public override void Effect(Selector_Enemy _opponent)
        {
            if(ArtifactManager.Instance.currentUseCard.count == Karin.CountType.Seven)
            {
                Debug.Log("¿€µø");
                int value = Random.Range(1, 6);
                value *= (Random.Range(0, 2) == 1) ? -1 : 1;

                DamageEffect.Instance.Damage(value, transform.parent.GetComponentInParent<Selector_Enemy>(), false);
            }
        }
    }

}
