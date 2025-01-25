using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
public class GameDataConfig : ScriptableObject
{
    public class PlayerData
    {
        public int Hp;
        public int Score;
        public int Power;
    }
    
    public class BubbleData
    {
        public string BubbleKey;
        public int Hp;
        public int Score;
    }
    
    public class ItemData
    {
        public string ItemKey;
        public Sprite Icon;
    }
    
    public PlayerData PData;
    public List<BubbleData> PlayerDataList;
    public List<ItemData> ItemDataList;
    
    public int PlayerLife = 3;
    public int PlayerDefaultAttackPower = 10;
    public int BubbleMaxLife = 100;
}
