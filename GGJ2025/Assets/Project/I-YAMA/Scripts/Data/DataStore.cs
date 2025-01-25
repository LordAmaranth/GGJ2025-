using System.Collections.Generic;
using GGJ.Common;
using R3;
using UnityEngine.Serialization;

namespace Project.GGJ2025
{
    /// <summary>
    /// データ共通化クラス
    /// </summary>
    public class DataStore
    {
        public class PlayerInfo
        {
            public int PlayerId;
            public readonly SerializableReactiveProperty<int> Hp = new ();
            public readonly SerializableReactiveProperty<int> Score = new ();
            
            public PlayerInfo(int playerId)
            {
                PlayerId = playerId;
                Hp.Value = 3;
                Score.Value = 0;
            }
        }
        
        public class BubbleInfo
        {
            public int Hp;
            public int Score;
        }
        
        public class ItemInfo
        {
            public string key;
            public string value;
        }
        
        //インスタンス
        private static DataStore instance;
        //インスタンスを外部から参照する用(getter)
        public static DataStore Instance => instance ??= new DataStore();

        public PlayerState PlayerState = PlayerState.None;
        public GameState GameState = GameState.Start;

        public List<PlayerInfo> PlayerInfos = new List<PlayerInfo>();
        public List<BubbleInfo> BubbleInfos = new List<BubbleInfo>();
        public List<ItemInfo> ItemInfos = new List<ItemInfo>();
    }
}