using System.Threading.Tasks;
using MagmaHeart.UIWindowPopupSystem;

namespace MagmaHeart.Core.TutorialSystem
{
    public class TutorialWindowPresenter : WindowPresenter
    {
        private TaskCompletionSource<bool> m_windowClosed;

        public override void Initialize(WindowData windowData)
        {
            base.Initialize(windowData);

            m_windowClosed = new TaskCompletionSource<bool>();
        }

        public Task GetTask() => m_windowClosed.Task;

        public void Close()
        {
            gameObject.SetActive(false);
            m_windowClosed.SetResult(true);
        }
    }
}