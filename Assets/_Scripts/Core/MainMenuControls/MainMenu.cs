using UnityEngine;
using UnityEngine.SceneManagement;

namespace MagmaHeart.Core.MainMenuControls
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}