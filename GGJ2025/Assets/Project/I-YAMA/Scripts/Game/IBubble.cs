using UnityEngine;

namespace Project.GGJ2025
{
    /// <summary>
    /// バブル状態
    /// </summary>
    public interface IBubble
    {
        /// <summary>
        /// リスポーン
        /// </summary>
        public void Respawn();

        /// <summary>
        /// 削除
        /// </summary>
        public void Delete();
    }
}
