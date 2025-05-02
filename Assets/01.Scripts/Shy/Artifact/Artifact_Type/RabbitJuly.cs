using UnityEngine;

namespace Shy
{
    public class RabbitJuly : ArtifactEffect
    {
        private Selector_Character user;

        public override void Effect(Selector_Character _opponent)
        {
            if(ArtifactManager.Instance.currentUseCard.count == Karin.CountType.Seven)
            {
                int value = Random.Range(1, 7);

                if(Random.Range(0, 2) == 1) HealthEffect.Instance.HealthEvent(value, user, false);
                else HealthEffect.Instance.HealthEvent(-value, _opponent, false);
            }
        }

        public override void Init()
        {
            user = transform.parent.GetComponentInParent<Selector_Character>();
        }
    }

}
