using System;
using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.Presentation.UI
{
    public class EscapeScreen : MonoBehaviour
    {
        private UserInput m_userInput;

        public void Initialize(UserInput userInput)
        {
            m_userInput = userInput;

            m_userInput.OnEscapeScreenButtonPress += ToggleDisplay;
        }

        public void Disable() => m_userInput.OnEscapeScreenButtonPress -= ToggleDisplay;

        private void ToggleDisplay(object _, EventArgs __) => gameObject.SetActive(!gameObject.activeSelf);

        public void Restart() => MagmaHeart.Restart();
        public void ExitGame() => MagmaHeart.ExitGame();
    }
}