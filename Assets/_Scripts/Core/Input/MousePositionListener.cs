using System;
using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class MousePositionListener : MonoBehaviour
    {
        private UserInput m_userInput;
        private Vector2 m_currentMouseScreenPosition;
        private Vector2 m_currentWorldMousePosition;

        public event EventHandler<OnMouseWorldPositionChangedEventArgs> OnMouseWorldPositionChanged;

        public void Initialize(UserInput userInput)
        {
            m_userInput = userInput;
            m_userInput.OnMousePositionChanged += HandleOnMousePositionChanged;
        }

        public void OnDisable()
        {
            m_userInput.OnMousePositionChanged -= HandleOnMousePositionChanged;
        }

        private void HandleOnMousePositionChanged(object obj, OnMousePositionChangedEventArgs args) => m_currentMouseScreenPosition = args.Position;

        private void Update()
        {
            Vector2 world = Camera.main.ScreenToWorldPoint(m_currentMouseScreenPosition);

            if (world != m_currentWorldMousePosition)
            {
                m_currentWorldMousePosition = world;
                OnMouseWorldPositionChangedEventArgs args = new OnMouseWorldPositionChangedEventArgs(world);
                OnMouseWorldPositionChanged?.Invoke(this, args);
            }
        }
    }
}

