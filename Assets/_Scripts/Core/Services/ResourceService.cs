using MagmaHeart.Abilities.Resources;
using MagmaHeart.Core.Abilities.Effects.Handlers;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Services
{
    public class ResourceService : IResourceService
    {
        private readonly GameWorld m_world;

        public ResourceService(GameWorld world)
        {
            m_world = world;
            Debug.LogWarning("Prototype code. TODO: Change this shit");
        }

        public void Spend(int entityId, ResourceId resource, int amount)
        {
            m_world.TryGetEntity(entityId, out Entity entity);

            // TODO: Prototype. This should be more generic and not hardcoded to energy.
            if (resource.Id == "Energy")
            {
                entity.Model.Energy.CurrentEnergy -= amount;
            }
        }
    }
}
