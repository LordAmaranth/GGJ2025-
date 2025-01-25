using GGJ.Common;

namespace Project.GGJ2025
{
    /// <summary>
    /// データ共通化クラス
    /// </summary>
    public class DataStore : SingletonMonoBehaviour<DataStore>
    {
        public PlayerState PState = PlayerState.None;

        protected override void Init()
        {
            base.Init();
            gameObject.name += "(Singleton)";
            DontDestroyOnLoad(this.gameObject);
        }
    }
}