using System;
using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class MouseHover
    {
        private readonly UserInput m_userInput;
        private IMouseHoverStrategy m_currentHoverStrategy;
        private readonly ActionMouseHoverStrategy m_actionMouseHoverStrategy;

        public event EventHandler<OnMouseHoverEventArgs> OnMouseHover;

        public MouseHover(UserInput userInput)
        {
            m_userInput = userInput;
            m_userInput.OnMousePositionChanged += HandleOnMousePositionChanged;

            m_actionMouseHoverStrategy = new ActionMouseHoverStrategy();
            m_currentHoverStrategy = m_actionMouseHoverStrategy;
        }

        public void Disable()
        {
            m_userInput.OnMousePositionChanged -= HandleOnMousePositionChanged;
        }

        private void HandleOnMousePositionChanged(object obj, OnMousePositionChangedEventArgs args)
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(args.Position);
            MouseHoverResult result = m_currentHoverStrategy.Evaluate(worldPosition);
        }
    }
}