using DG.Tweening;
using GGJ.Common;
using KanKikuchi.AudioManager;
using R3;
using UnityEngine;
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
            
            // ボタンのクリック
            _startButton
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SEManager.Instance.Play(SEPath.SYSTEM20);
                    Debug.Log("Start Game!");

                    DataStore.Instance.PlayerState = PlayerState.Alive;

                    SceneType.Game.Load();
                })
                .AddTo(this);
        }

        // Update is called once per frame
        private void Update()
        {
        
        }
    }
}
