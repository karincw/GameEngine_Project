using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public class TripleCard_ART : ArtifactEffect
    {
        private int cnt = 0;
        public override void Effect()
        {
            if(++cnt == 3)
            {
                cnt = 0;
                Debug.Log("적에게 3의 피해");
            }
        }
    }
}
