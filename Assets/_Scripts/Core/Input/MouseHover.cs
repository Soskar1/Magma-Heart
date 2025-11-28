using System;
using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class MouseHover : MonoBehaviour
    {
        public event EventHandler OnMouseEnterEvent;
        public event EventHandler OnMouseExitEvent;

        private void OnMouseEnter() => OnMouseEnterEvent?.Invoke(this, EventArgs.Empty);

        private void OnMouseExit() => OnMouseExitEvent?.Invoke(this, EventArgs.Empty);
    }
}