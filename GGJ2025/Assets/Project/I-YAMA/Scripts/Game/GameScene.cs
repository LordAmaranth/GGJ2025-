using GGJ.Common;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Project.GGJ2025
{
    public class GameScene : SingletonMonoBehaviour<GameScene>
    {
        public Button _pauseButton;
        
        private void Start()
        {
            Debug.Log(DataStore.Instance.PlayerState);

            // プレイヤー追加
            var playerInfos = DataStore.Instance.PlayerInfos;
            playerInfos.Add(new DataStore.PlayerInfo(1));
            playerInfos.Add(new DataStore.PlayerInfo(2));
            playerInfos.Add(new DataStore.PlayerInfo(3));
            playerInfos.Add(new DataStore.PlayerInfo(4));

            // プレイヤー情報変更監視登録
            for (int index = 0, max = playerInfos.Count; index < max; index++)
            {
                var player = playerInfos[index];
                // HP変更監視
                player.Hp
                    .Skip(0)
                    .Subscribe(hp =>
                    {
                        Debug.Log($"playerId:{player.PlayerId} Hp:{hp}");
                    })
                    .AddTo(this);
            }

            // ボタンのクリック
            _pauseButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    playerInfos[0].Hp.Value -= 1;
                })
                .AddTo(this);

        }
    }
}
