using System.Linq;
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
        public GameObject[] helpWindow;

        private int helpIndex = 0;
        private int rankCount = 0;
        
        private GameState oldGameState = GameState.Title;
        private Vector2 gameStartPos = new Vector2(-10, 10);
        
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
                    join.SetActive(state == GameState.Join || state == GameState.Help);
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
                    if (!playerInputManager.joiningEnabled)
                    {
                        // 参加有効
                        playerInputManager.EnableJoining();
                    }
                    // プレイヤー登録画面
                    DataStore.Instance.PlayerInfos.ForEach(playerInfo =>
                    {
                        // プレイヤー位置初期化
                        playerInfo.Player.gameObject.transform.position = Vector3.zero;
                    });
                    break;
                case GameState.Start:
                    if (playerInputManager.joiningEnabled)
                    {
                        // 参加無効
                        playerInputManager.DisableJoining();
                    }
                    // 順位リセット        
                    rankCount = DataStore.Instance.PlayerInfos.Count;
                    // ゲーム開始
                    DataStore.Instance.PlayerInfos.ForEach(playerInfo =>
                    {
                        // プレイヤー位置ランダム
                        var randPosition = new Vector3(Random.Range(gameStartPos.x, gameStartPos.y), 10f, 0f);
                        playerInfo.Player.gameObject.transform.position = randPosition;
                    });
                    break;
                case GameState.Spawn:
                    // リスポーン
                    DataStore.Instance.PlayerInfos.ForEach(playerInfo =>
                    {
                        // スコアリセット
                        playerInfo.Score.Value = 0;
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
                    zoomCamera2D.targets.Clear();
                    // リザルト
                    DataStore.Instance.PlayerInfos.ForEach(playerInfo =>
                    {
                        // プレイヤー位置初期化
                        playerInfo.Player.gameObject.transform.position = Vector3.zero;
                        playerInfo.PState.Value = PlayerState.None;
                        // 操作復元
                        playerInfo.Player.ReenableControls();
                        // カメラ追従再登録
                        zoomCamera2D.targets.Add(playerInfo.Player.transform);
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
                
            // 状態変更監視
            player.PState
                .Skip(1)
                .Subscribe(state =>
                {
                    var dataStore = DataStore.Instance;
                    var playerInfos = dataStore.PlayerInfos;
                    // 全員死亡したらゲーム終了 リザルト遷移
                    if (playerInfos.All(x => x.PState.Value == PlayerState.Death))
                    {
                        // 全員死亡
                        dataStore.GameState.Value = GameState.Result;
                        return;
                    }
                    
                    Debug.Log($"playerId:{player.PlayerId} PState:{state}");
                    var p = player.Player;
                    switch (state)
                    {
                        case PlayerState.Spawn:
                            // 登場
                            p.ReenableControls();
                            p.gameObject.SetActive(true);
                            var py = zoomCamera2D.transform.position.y;
                            player.Player.transform.position = new Vector3(Random.Range(gameStartPos.x, gameStartPos.y), py + 10f, 0f);
                            // カメラ追従追加
                            zoomCamera2D.targets.Add(p.transform);
                            break;
                        case PlayerState.Death:
                            // ランク割当
                            player.Rank.Value = rankCount;
                            rankCount--;
                            
                            // 死亡
                            p.DisableControls();
                            // オブジェクト非表示 仮
                            // p.gameObject.SetActive(false);
                            // 死亡したらカメラ追従から外す
                            zoomCamera2D.targets.Remove(p.transform);
                            break;
                    }
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
                    
                    if (playerInfos.Any(x => x.AState.Value == AreaState.HelpPointArea))
                    {
                        // 誰かがヘルプエリアにいる場合
                        helpWindow[helpIndex].SetActive(true);
                        helpIndex++;
                        if (helpIndex >= helpWindow.Length)
                        {
                            helpIndex = 0;
                        }
                    }
                    else
                    {
                        foreach (var o in helpWindow)
                        {
                            o.SetActive(false);
                        }
                    }

                    if (area == AreaState.DeadZonePointArea && player.PState.Value != PlayerState.Death)
                    {
                        // 死亡エリアに入った場合
                        player.PState.Value = PlayerState.Death;
                    }
                })
                .AddTo(player.Player.gameObject);
        }
    }
}
