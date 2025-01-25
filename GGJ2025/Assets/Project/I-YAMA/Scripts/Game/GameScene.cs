using GGJ.Common;
using UnityEngine;

namespace Project.GGJ2025
{
    public class GameScene : SingletonMonoBehaviour<GameScene>
    {
        private void Start()
        {
            Debug.Log(DataStore.Instance.PState);
        }

        private void Update()
        {

        }
    }
}
