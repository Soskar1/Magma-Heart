using MagmaHeart.Collections;
using MagmaHeart.Core.Entities;
using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnTurnSwitchedEventArgs : EventArgs
    {
        public CircularList<Entity> CurrentTurnOrder { get; init; }
        public Entity CurrentEntity => CurrentTurnOrder.Head;

        public OnTurnSwitchedEventArgs(CircularList<Entity> turnOrder) => CurrentTurnOrder = turnOrder;
    }
}