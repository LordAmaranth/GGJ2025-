using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ.Common
{
    public static partial class CommonExtensions
    {
        /// <summary>
        /// Scene Load
        /// </summary>
        public static void Load(this SceneType me)
        {
            SceneManager.LoadScene(me.ToString());
        }

        public static Vector3 GetPositionRate(this Vector3 position)
        {
            return new Vector3(position.x / Screen.width, position.y / Screen.height, 0f);
        }
    }
}