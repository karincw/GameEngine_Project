using Shy;
using UnityEngine;

namespace Karin
{

    public class SettingPanel : MonoBehaviour
    {
        [SerializeField] private string _titleSceneName;

        public void Help()
        {
            SoundManager.Instance.rule.SetActive(true);
        }

        public void GoToTitle()
        {
            StageManager.Instance.StageInit();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }

}