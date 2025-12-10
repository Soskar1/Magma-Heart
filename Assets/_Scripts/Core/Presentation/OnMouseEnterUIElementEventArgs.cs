using System;
using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class OnMouseEnterUIElementEventArgs : EventArgs
    {
        public GameObject UIElement { get; init; }
        public OnMouseEnterUIElementEventArgs(GameObject uiElement) => UIElement = uiElement;
    }
}
