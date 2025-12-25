using System.Collections.Generic;
using MagmaHeart.AI.Actions;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal record EntityInitializationData(Vector3Int Position, bool IsPlayer, int MaxHealth, List<ActionData> Actions)
    {
        public EntityModel GetModel()
        {
            EntityStats stats = new EntityStats(MaxHealth);
            EntityData data = new EntityData("", stats, Actions == null ? new List<ActionData>() : Actions);
            return new EntityModel(data, () => Position, IsPlayer);
        }
    }
}