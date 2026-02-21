using System;

namespace MagmaHeart.Core.Input.Mouse
{
    [Flags]
    public enum HoverResultType
    {
        None = 0,
        Tile = 1,
        Entity = 2,
        UI = 4
    }
}
