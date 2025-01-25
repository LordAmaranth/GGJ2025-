using UnityEngine;

namespace Project.GGJ2025
{
    /// <summary>
    /// プレイヤー
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// リスポーン
        /// </summary>
        public void Respawn();
        
        /// <summary>
        /// 移動
        /// </summary>
        public void  Move();

        /// <summary>
        /// ジャンプ
        /// </summary>
        public void Jump();
        
        /// <summary>
        /// 死亡
        /// </summary>
        public void Death();
        
        /// <summary>
        /// 攻撃
        /// </summary>
        public void Attack();
        
        /// <summary>
        /// 攻撃長押し
        /// </summary>
        public void LongAttack();
    }
}
