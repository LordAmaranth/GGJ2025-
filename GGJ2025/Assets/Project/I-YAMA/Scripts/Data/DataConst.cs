// -----------------------------------------------------------------
// <copyright file="DataConst.cs">
// Copyright © -----------. All rights reserved.
// </copyright>
// -----------------------------------------------------------------

namespace Project.GGJ2025
{
    /// <summary>
    /// プレイヤー状態
    /// </summary>
    public enum PlayerState
    {
        None,
        // 停止
        Pause,
        // 登場
        Spawn,
        // リスポーン
        Respawn,
        // 生きている
        Alive,
        // 死んでいる
        Death
    }
    
    public enum AreaState
    {
        None,
        // スタート
        StartPointArea,
        // ヘルプ中
        HelpPointArea,
        // もう一回
        RetryPointArea,
        // 戻る
        ReturnPointArea,
        // 死亡ゾーン
        DeadZonePointArea,
    }
    
    /// <summary>
    /// バブル状態
    /// </summary>
    public enum BubbleState
    {
        None,
        Alive,
        Death
    }

    public enum GameState
    {
        Title,
        Help,
        Join,
        Start,
        Spawn,
        Pause,
        End,
        Result,
    }
    
    /// <summary>
    /// 定数
    /// </summary>
    public static class DataConst
    {
        public const int PlayerLife = 3;
        public const int PlayerDefaultAttackPower = 10;
        public const int BubbleMaxLife = 100;
    }
}