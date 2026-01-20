using MagmaHeart.Core.Input;
using System;
using UnityEngine;

namespace MagmaHeart.Core.Presentation.UI
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] private SeedDisplay m_seedDisplay;
        private UserInput m_userInput;

        public void Initialize(UserInput userInput, int seed)
        {
            m_userInput = userInput;
            m_seedDisplay.Initialize(seed);

            m_userInput.OnDebugButtonPress += ToggleDisplay;
        }

        public void Disable() => m_userInput.OnDebugButtonPress -= ToggleDisplay;

        private void ToggleDisplay(object obj, EventArgs args) => gameObject.SetActive(!gameObject.activeSelf);
    }
}