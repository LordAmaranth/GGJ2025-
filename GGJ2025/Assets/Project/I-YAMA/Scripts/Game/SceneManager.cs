using GGJ.Common;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.GGJ2025
{
    /// <summary>
    /// 場面制御クラス
    /// </summary>
    public class SceneManager : SingletonMonoBehaviour<SceneManager>
    {
        public PlayerInputManager playerInputManager;
        public ZoomCamera2D zoomCamera2D;
        public GameObject title;
        public GameObject join;
        public GameObject battle;
        public GameObject result;

        private void Awake()
        {
            // PlayerInputManager からプレイヤー登録最大数設定
            DataStore.Instance.Init(playerInputManager.maxPlayerCount);
            
            // ゲーム状態監視
            DataStore.Instance.GameState
                .Skip(1)
                .Subscribe(state =>
                {
                    Debug.Log($"state:{state}");
                    // タイトルUI表示切替
                    title.SetActive(state == GameState.Title);
                    join.SetActive(state == GameState.Join);
                    battle.SetActive(state == GameState.Start || state == GameState.Spawn || state == GameState.Pause || state == GameState.End);
                    result.SetActive(state == GameState.Result);
                })
                .AddTo(this);
            
            DataStore.Instance.GameState.Value = GameState.Title;
        }

        private void Start()
        {
            // プレイヤー登録
            DataStore.Instance.OnJoin
                .Subscribe(playerInfo =>
                {
                    if (DataStore.Instance.GameState.Value == GameState.Title)
                    {
                        DataStore.Instance.GameState.Value = GameState.Join;
                    }
                    Debug.Log($"playerId:{playerInfo.PlayerId} join");
                    zoomCamera2D.targets.Add(playerInfo.Player.transform);
                })
                .AddTo(this);
            // プレイヤー削除
            DataStore.Instance.OnLeft
                .Subscribe(playerInfo =>
                {
                    Debug.Log($"playerId:{playerInfo.PlayerId} Left");
                    zoomCamera2D.targets.Remove(playerInfo.Player.transform);
                })
                .AddTo(this);
            var playerInfos = DataStore.Instance.PlayerInfos;
            // プレイヤー情報変更監視登録
            for (int index = 0, max = playerInfos.Count; index < max; index++)
            {
                var player = playerInfos[index];
                
                // HP変更監視
                player.Hp
                    .Skip(1)
                    .Subscribe(hp =>
                    {
                        Debug.Log($"playerId:{player.PlayerId} Hp:{hp}");
                    })
                    .AddTo(this);
            }
        }
    }
}
