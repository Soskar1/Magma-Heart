using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Abilities;
using System;

namespace MagmaHeart.Core.Abilities.Selection
{
    public class OnAbilitySelectedEventArgs : EventArgs
    {
        public AbilityPlan Plan { get; init; }
        public HoverResult HoverResult { get; init; }

        public OnAbilitySelectedEventArgs(AbilityPlan plan, HoverResult hoverResult)
        {
            Plan = plan;
            HoverResult = hoverResult;
        }
    }
}
