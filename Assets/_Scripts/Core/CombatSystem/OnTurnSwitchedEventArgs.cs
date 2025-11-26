using MagmaHeart.Core.Entities.Presenters;
using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnTurnSwitchedEventArgs : EventArgs
    {
        public EntityPresenter Entity { get; init; }

        public OnTurnSwitchedEventArgs(EntityPresenter entity) => Entity = entity;
    }
}