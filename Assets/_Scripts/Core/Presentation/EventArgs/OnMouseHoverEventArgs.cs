using System;
using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class OnMouseHoverEventArgs : EventArgs
    {
        public MouseHoverResult HoverResult { get; init; }
    }
}