using System;

namespace MagmaHeart.Core.Input.Mouse
{
    public class OnMouseHoverEventArgs : EventArgs
    {
        public HoverResult HoverResult { get; init; }
        public OnMouseHoverEventArgs(HoverResult result) => HoverResult = result;
    }
}
