using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public class TripleCard_ART : ArtifactEffect
    {
        private int cnt = 0;


        public override void Effect(Selector_Enemy _opponent)
        {
            if(++cnt == 12)
            {
                HealthEffect.Instance.HealthEvent(3, transform.parent.GetComponentInParent<Selector_Enemy>(), false);
                cnt = 0;
                Debug.Log("자신 3 회복");
            }
        }

        public override void Init()
        {
            cnt = 0;
        }
    }
}
