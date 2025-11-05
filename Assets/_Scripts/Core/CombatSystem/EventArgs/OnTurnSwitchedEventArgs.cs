using MagmaHeart.Core.Entities;
using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnTurnSwitchedEventArgs : EventArgs
    {
        public Entity CurrentEntity { get; init; }

        public OnTurnSwitchedEventArgs(Entity entity) => CurrentEntity = entity;
    }
}