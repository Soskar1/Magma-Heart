using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Core.SceneLoading;
using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class InputInstaller : IInstaller
    {
        private UserInput m_userInput;
        private MouseListener m_mouseListener;
        private MouseHoverEngine m_hoverEngine;

        public InputContext Install(MouseListener mouseListenerPrefab)
        {
            m_userInput = new UserInput();

            m_mouseListener = GameObject.Instantiate(mouseListenerPrefab);
            m_mouseListener.Initialize(m_userInput);

            m_hoverEngine = new MouseHoverEngine(m_mouseListener);

            return new InputContext(m_userInput, m_mouseListener, m_hoverEngine);
        }

        public void Dispose()
        {
            m_userInput.Disable();
            m_mouseListener.Disable();
            m_hoverEngine.Disable();
        }
    }
}
