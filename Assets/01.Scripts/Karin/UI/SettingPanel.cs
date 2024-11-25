using Shy;
using UnityEngine;

namespace Karin
{

    public class SettingPanel : MonoBehaviour
    {
        [SerializeField] private string _titleSceneName;

        public void Help()
        {

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