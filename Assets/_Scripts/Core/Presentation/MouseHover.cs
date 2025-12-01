using System;
using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class MouseHover
    {
        private readonly UserInput m_userInput;

        public event EventHandler<OnMouseHoverEventArgs> OnHoverWorldPosition;

        public MouseHover(UserInput userInput)
        {
            m_userInput = userInput;
            m_userInput.OnMousePositionChanged += HandleOnMousePositionChanged;
        }

        private void HandleOnMousePositionChanged(object obj, OnMousePositionChangedEventArgs args)
        {
            Vector2 world = Camera.main.ScreenToWorldPoint(args.Position);

            OnMouseHoverEventArgs mouseHoverArgs = new OnMouseHoverEventArgs(world);
            OnHoverWorldPosition?.Invoke(this, mouseHoverArgs);
        }
    }
}