using System;

namespace MagmaHeart.Core.Input
{
    public class OnMouseScrollEventArgs : EventArgs
    {
        public float MouseScroll { get; init; }

        public OnMouseScrollEventArgs(float mouseScroll) => MouseScroll = mouseScroll;
    }
}
