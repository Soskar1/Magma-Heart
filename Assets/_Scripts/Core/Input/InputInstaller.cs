using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Core.SceneLoading;
using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class InputInstaller : IInstaller
    {
        private UserInput m_userInput;
        private MouseListener m_mouseListener;

        public InputContext Install(MouseListener mouseListenerPrefab)
        {
            m_userInput = new UserInput();

            m_mouseListener = GameObject.Instantiate(mouseListenerPrefab);
            m_mouseListener.Initialize(m_userInput);

            return new InputContext(m_userInput, m_mouseListener);
        }

        public void Dispose()
        {
            m_userInput.Disable();
            m_mouseListener.Disable();
        }
    }
}
