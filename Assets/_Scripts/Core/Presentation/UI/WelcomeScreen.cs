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

        public Task GetTask() => m_welcomeScreenClosed.Task;

        public void Close()
        {
            gameObject.SetActive(false);
            m_welcomeScreenClosed.SetResult(true);
        }

        // TODO: implement redirect buttons
    }
}