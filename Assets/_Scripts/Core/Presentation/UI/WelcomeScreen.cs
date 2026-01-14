using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Presentation.UI
{
    public class WelcomeScreen : MonoBehaviour
    {
        private TaskCompletionSource<bool> m_welcomeScreenClosed;

        public void Open()
        {
            gameObject.SetActive(true);

            m_welcomeScreenClosed = new TaskCompletionSource<bool>();
        }

        public Task GetTask()
        {
            if (m_welcomeScreenClosed != null)
                return m_welcomeScreenClosed.Task;

            return Task.CompletedTask;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            m_welcomeScreenClosed.SetResult(true);
        }

        public void OpenYoutubeChannel() => Application.OpenURL("https://www.youtube.com/@Soskar");
        public void OpenX() => Application.OpenURL("https://x.com/SoskarDev");
        public void OpenDiscordServer() => Application.OpenURL("https://discord.gg/5XjyvhXbbn");
    }
}