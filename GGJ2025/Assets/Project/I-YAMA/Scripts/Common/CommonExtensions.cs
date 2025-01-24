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
    }
}