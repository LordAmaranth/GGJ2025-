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
        Alive,
        Death
    }

    public enum GameState
    {
        Start,
        Spawn,
        Pause,
        End,
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