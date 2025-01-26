using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace Project.GGJ2025
{
    /// <summary>
    /// データ共通化クラス
    /// </summary>
    public class DataStore
    {
        /// <summary>
        /// プレイヤー情報
        /// </summary>
        public class PlayerInfo
        {
            // プレイヤーID 0-4
            public int PlayerId;
            // プレイヤー状態
            public readonly SerializableReactiveProperty<PlayerState> PState = new ();
            // エリア状態
            public readonly SerializableReactiveProperty<AreaState> AState = new ();
            // HP
            public readonly SerializableReactiveProperty<int> Hp = new ();
            // スコア
            public readonly SerializableReactiveProperty<int> Score = new ();
            // ランク
            public readonly SerializableReactiveProperty<int> Rank = new ();
            // 入力デバイス
            public Transform Transform;
            public Player Player;
            
            public PlayerInfo(int playerId, Player player)
            {
                Player = player;
                PlayerId = playerId;
                PState.Value = PlayerState.None;
                AState.Value = AreaState.None;
                Hp.Value = 3;
                Score.Value = 0;
            }
        }
        
        /// <summary>
        /// バブル情報
        /// </summary>
        public class BubbleInfo
        {
            public readonly SerializableReactiveProperty<BubbleState> State = new ();
            public readonly SerializableReactiveProperty<int> Hp = new ();
            public readonly SerializableReactiveProperty<int>  ItemID = new ();
        }
        
        /// <summary>
        /// アイテム情報
        /// </summary>
        public class ItemInfo
        {
            public string key;
            public string value;
        }
        
        // -------- private ---------
        
        // 割り当て可能プレイヤーID
        private List<int> playerIds = new List<int>();
        // 割り当て済みプレイヤーID
        private HashSet<int> assignedIds = new HashSet<int>();

        public int helpIndex = 0;
        
        // 参加通知
        private Subject<PlayerInfo> joinSubject = new Subject<PlayerInfo>();
        // 退場通知
        private Subject<PlayerInfo>  leftSubject = new Subject<PlayerInfo>();
        
        //インスタンス
        private static DataStore instance;
        
        // -------- public ---------
        
        //インスタンスを外部から参照する用(getter)
        public static DataStore Instance => instance ??= new DataStore();
        
        // ゲーム状態
        public readonly SerializableReactiveProperty<GameState> GameState = new ();

        // 参加通知
        public Subject<PlayerInfo> OnJoin => joinSubject;
        // 退場通知
        public Subject<PlayerInfo> OnLeft => leftSubject;
        
        // プレイヤー情報
        public List<PlayerInfo> PlayerInfos { get; } = new ();
        // バブル情報
        public List<BubbleInfo> BubbleInfos { get; } = new ();
        // アイテム情報
        public List<ItemInfo> ItemInfos { get; } = new ();

        // -------- private function ---------
        
        /// <summary>
        /// 未割り当てのプレイヤーIDを取得
        /// </summary>
        private List<int> GetUnassignedIds()
        {
            return playerIds.Where(playerId => !assignedIds.Contains(playerId)).ToList();
        }
        
        /// <summary>
        /// プレイヤー情報取得
        /// </summary>
        private PlayerInfo GetPlayerInfo(Player player)
        {
            return PlayerInfos.FirstOrDefault(info => info.Player == player);
        }
        
        // -------- public function ---------
        
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(int maxPlayerCount)
        {
            Debug.Log($"Init PlayerCount : {maxPlayerCount}");
            playerIds.Clear();
            for (int i = 0; i < maxPlayerCount; i++)
            {
                playerIds.Add(i);
            }
            Debug.Log($"playerIds: {string.Join(",", playerIds)}");
        }
        
        /// <summary>
        /// プレイヤー追加
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(Player player)
        {
            var playerId = GetUnassignedIds().DefaultIfEmpty(-1).FirstOrDefault();
            if (playerId < 0)
            {
                Debug.LogError("PlayerId is full");
                return;
            }
            // 割り当て済みプレイヤーIDに追加
            assignedIds.Add(playerId);
            Debug.Log($"Add PlayerID : {playerId}");
            var playerInfo = new PlayerInfo(playerId, player);
            PlayerInfos.Add(playerInfo);
            joinSubject.OnNext(playerInfo);
        }
        
        /// <summary>
        /// プレイヤー削除
        /// </summary>
        public void RemovePlayer(Player player)
        {
            var playerInfo = PlayerInfos.FirstOrDefault(info => info.Player == player);
            if (playerInfo == null)
            {
                Debug.LogError("PlayerInfo is not found");
                return;
            }
            Debug.Log($"Remove PlayerID: {playerInfo.PlayerId}");
            assignedIds.Remove(playerInfo.PlayerId);
            leftSubject.OnNext(playerInfo);
            PlayerInfos.Remove(playerInfo);
        }
        
        /// <summary>
        /// エリア別状態変更
        /// </summary>
        public void HitChangePlayerState(Player player, string hitAreaTag)
        {
            // Debug.Log($"HitChangePlayerState player:{player} {player} hitAreaTag:{hitAreaTag}");
            var playerInfo = GetPlayerInfo(player);
            if (playerInfo == null)
            {
                Debug.Log($"Info is not found");
                return;
            }
            playerInfo.AState.Value = (AreaState)System.Enum.Parse(typeof(AreaState), hitAreaTag);;
            // Debug.Log($"playerInfo.AState.Value:{playerInfo.AState.Value}");
        }
    }
}