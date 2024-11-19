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
            if(++cnt == 3)
            {
                _opponent.health -= 3;
                cnt = 0;
                Debug.Log("적에게 3의 피해");
            }
        }

        public override void Init()
        {
            cnt = 0;
        }
    }
}
