using System;
using UnityEngine;

namespace MagmaHeart.Core.Input.Mouse
{
    public class OnMouseEnterUIElementEventArgs : EventArgs
    {
        public GameObject UIElement { get; init; }
        public OnMouseEnterUIElementEventArgs(GameObject uiElement) => UIElement = uiElement;
    }
}
