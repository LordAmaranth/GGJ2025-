using KanKikuchi.AudioManager;
using UnityEngine.UI;

namespace Project.GGJ2025
{
    public class TitleScene : GGJ.Common.SingletonMonoBehaviour<TitleScene>
    {
        public Button _startButton;
        
        private void Start()
        {
            // BGM再生
            BGMManager.Instance.Play(BGMPath.FANTASY14);
        }
    }
}