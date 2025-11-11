using MagmaHeart.Core.Entities;
using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnAttackEventArgs : EventArgs
    {
        public EntityModel Target { get; init; }

        public OnAttackEventArgs(EntityModel target) => Target = target;
    }
}