using System;

namespace MagmaHeart.Abilities.Targeting
{
    [Flags]
    public enum TargetKind
    {
        None = 0,
        Entity = 1,
        Position = 2,
        Path = 4
    }
}
