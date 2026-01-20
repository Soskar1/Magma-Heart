using UnityEngine;
using UnityEngine.SceneManagement;

namespace MagmaHeart.Core
{
    public static class MagmaHeart
    {
        public static void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        public static void ExitGame() => Application.Quit();
    }
}
