using System;

namespace MagmaHeart.Core.Input.Mouse
{
    public class OnHoverResultChangedEventArgs : EventArgs
    {
        public HoverResult HoverResult { get; init; }
        public OnHoverResultChangedEventArgs(HoverResult hoverResult) => HoverResult = hoverResult;
    }
}
