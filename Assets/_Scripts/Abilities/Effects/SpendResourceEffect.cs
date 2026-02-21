using MagmaHeart.Abilities.Resources;
using MagmaHeart.Abilities.Targeting;
using System;
using System.Collections.Generic;

namespace MagmaHeart.Abilities.Effects
{
    public sealed record SpendResourceEffect(int ExecutorId, ResourceId Resource, int Amount) : AbilityEffect(ExecutorId);
}
