using MagmaHeart.Core.Entities;
using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnEntityDiedEventArgs : EventArgs
    {
        public EntityModel Model { get; init; }
        public OnEntityDiedEventArgs(EntityModel model) => Model = model;
    }
}
