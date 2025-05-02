using UnityEngine;

namespace Shy
{
    public class LuckyChip : ArtifactEffect
    {
        private int cnt = 0;
        private Selector_Character user;

        public override void Effect(Selector_Character _opponent)
        {
            if(++cnt == 7)
            {
                HealthEffect.Instance.HealthEvent(3, user, false);
                cnt = 0;
            }
        }

        public override void Init()
        {
            cnt = 0;
            user = transform.parent.GetComponentInParent<Selector_Character>();
        }
    }
}
