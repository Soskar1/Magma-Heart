using System;

namespace MagmaHeart.Core.Entities
{
    public class OnDeathEventArgs : EventArgs
    {
        public EntityModel Model { get; init; }
        public OnDeathEventArgs(EntityModel model) => Model = model;
    }
}
