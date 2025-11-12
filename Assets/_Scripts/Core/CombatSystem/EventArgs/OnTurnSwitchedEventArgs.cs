using MagmaHeart.Core.Entities;
using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnTurnSwitchedEventArgs : EventArgs
    {
        public Entity Entity { get; init; }

        public OnTurnSwitchedEventArgs(Entity entity) => Entity = entity;
    }
}