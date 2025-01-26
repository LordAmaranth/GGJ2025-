using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        
        private GameState oldGameState = GameState.Title;
        
        /// <inheritdoc/>>
        protected override void Init()
        {
            base.Init();
            
            // PlayerInputManager からプレイヤー登録最大数設定
            DataStore.Instance.Init(playerInputManager.maxPlayerCount);
            
            // ゲーム状態監視
            DataStore.Instance.GameState
                .Subscribe(state =>
                {
                    Debug.Log($"state:{state}");
                    // タイトルUI表示切替
                    title.SetActive(state == GameState.Title);
                    join.SetActive(state == GameState.Join);
                    battle.SetActive(state == GameState.Start || state == GameState.Spawn || state == GameState.Pause || state == GameState.End);
                    result.SetActive(state == GameState.Result);

                    // ゲーム状態切り替え
                    if (oldGameState != state)
                    {
                        oldGameState = state;
                        ChangeGameState(state);
                    }
                })
                .AddTo(this);
            
            DataStore.Instance.GameState.Value = GameState.Title;
            Debug.Log($"Set state:{DataStore.Instance.GameState.Value}");
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
                    SetEvent(playerInfo);
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
        }
        
        /// <summary>
        /// ゲーム状態変更時の処理
        /// </summary>
        private void ChangeGameState(GameState state)
        {
            switch (state)
            {
                case GameState.Title:
                    // タイトル画面
                    break;
                case GameState.Join:
                    // プレイヤー登録画面
                    DataStore.Instance.PlayerInfos.ForEach(playerInfo =>
                    {
                        // プレイヤー位置初期化
                        playerInfo.Player.gameObject.transform.position = Vector3.zero;
                    });
                    break;
                case GameState.Start:
                    // ゲーム開始
                    DataStore.Instance.PlayerInfos.ForEach(playerInfo =>
                    {
                        // プレイヤー位置ランダム
                        var randPosition = new Vector3(Random.Range(-10f, 10f), 10f, 0f);
                        playerInfo.Player.gameObject.transform.position = randPosition;
                    });
                    break;
                case GameState.Spawn:
                    // リスポーン
                    DataStore.Instance.PlayerInfos.ForEach(playerInfo =>
                    {
                        // プレイヤー位置初期化
                        playerInfo.Player.gameObject.transform.position = Vector3.zero;
                    });
                    break;
                case GameState.Pause:
                    // 一時停止
                    break;
                case GameState.End:
                    // ゲーム終了
                    DataStore.Instance.PlayerInfos.ForEach(playerInfo =>
                    {
                        // プレイヤー位置初期化
                        playerInfo.Player.gameObject.transform.position = Vector3.zero;
                    });
                    break;
                case GameState.Result:
                    // リザルト
                    DataStore.Instance.PlayerInfos.ForEach(playerInfo =>
                    {
                        // プレイヤー位置初期化
                        playerInfo.Player.gameObject.transform.position = Vector3.zero;
                    });
                    break;
            }
        }

        /// <summary>
        /// 監視イベント登録
        /// </summary>
        private void SetEvent(DataStore.PlayerInfo player)
        {
            // HP変更監視
            player.Hp
                .Skip(1)
                .Subscribe(hp =>
                {
                    Debug.Log($"playerId:{player.PlayerId} Hp:{hp}");
                })
                .AddTo(player.Player.gameObject);
                
            // エリア変更監視
            player.AState
                .Skip(1)
                .Subscribe(area =>
                {
                    var dataStore = DataStore.Instance;
                    var playerInfos = dataStore.PlayerInfos;
                    if (playerInfos.All(x => x.AState.Value == AreaState.StartPointArea))
                    {
                        // 全員スタートエリアにいる場合
                        dataStore.GameState.Value = GameState.Start;
                    }
                    else if (playerInfos.All(x => x.AState.Value == AreaState.RetryPointArea))
                    {
                        // 全員リトライエリアにいる場合
                        dataStore.GameState.Value = GameState.Spawn;
                    }
                    else if (playerInfos.All(x => x.AState.Value == AreaState.ReturnPointArea))
                    {
                        // 全員リターンエリアにいる場合
                        dataStore.GameState.Value = GameState.Join;
                    }
                    else if (playerInfos.Any(x => x.AState.Value == AreaState.HelpPointArea))
                    {
                        // 誰かがヘルプエリアにいる場合
                        dataStore.GameState.Value = GameState.Help;
                    }
                })
                .AddTo(player.Player.gameObject);
        }
    }
}
