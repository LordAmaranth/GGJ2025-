using GGJ.Common;

namespace Project.GGJ2025
{
    public class DataStore : SingletonMonoBehaviour<DataStore>
    {
        public int PlayerLife = 3;
        public int PlayerDefaultAttackPower = 10;
        public int  BubbleMaxLife = 100;
    }
}