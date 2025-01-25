using GGJ.Common;
using UnityEngine.Serialization;

namespace Project.GGJ2025
{
    /// <summary>
    /// データ共通化クラス
    /// </summary>
    public class DataStore : SingletonMonoBehaviour<DataStore>
    {
        public PlayerState PlayerState = PlayerState.None;

        public GameState GameState = GameState.Start;

        protected override void Init()
        {
            base.Init();
            gameObject.name += "(Singleton)";
            DontDestroyOnLoad(this.gameObject);
        }
    }
}