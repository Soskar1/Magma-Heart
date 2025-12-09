using MagmaHeart.Core.Entities;
using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnEntityDiedEventArgs : EventArgs
    {
        public Entity DiedEntity { get; init; }
        public OnEntityDiedEventArgs(Entity model) => DiedEntity = model;
    }
}
