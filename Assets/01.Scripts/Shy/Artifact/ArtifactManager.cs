namespace Shy
{
    public enum EVENT_TYPE
    {
        NONE = 0,
        TURN_START,
        TURN_END,
        GET_DAMAGE, //피해를 입을 때
        SET_DAMAGE, //데미지 스택을 쌓았을 때
        CAUSE_DAMAGE, //피해를 줬을 때
        STAGE_START,
        STAGE_END,
        USE_CARD, //카드를 냈을 때
        GET_CARD, //카드를 얻었을 때
    }

    public class ArtifactManager : MonoSingleton<ArtifactManager>
    {
        public void OnEvent(EVENT_TYPE _pType = EVENT_TYPE.NONE, EVENT_TYPE _eType = EVENT_TYPE.NONE)
        {
            if(_pType != EVENT_TYPE.NONE)
            {
                foreach (Artifact item in StageManager.Instance.playerCard.artifacts)
                {
                    for (int i = 0; i < item.effects.Length; i++)
                        if (item.effects[i].eType == _pType) item.effects[i].Effect();
                }
            }

            if (_eType != EVENT_TYPE.NONE)
            {
                foreach (Artifact item in StageManager.Instance.enemyCard.artifacts)
                {
                    for (int i = 0; i < item.effects.Length; i++)
                        if (item.effects[i].eType == _eType)
                        {
                            item.effects[i].Effect();
                            break;
                        }
                }
            }
        }
    }
}
