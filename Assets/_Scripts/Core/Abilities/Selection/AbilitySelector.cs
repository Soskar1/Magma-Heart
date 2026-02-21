using MagmaHeart.Abilities;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Selection
{
    public abstract class AbilitySelector
    {
        public AbilitySelector Next { get; set; }

        public AbilityPlan GetAbility(GameWorld world, EntityModel executor, Vector3 worldPosition)
        {
            AbilityPlan result = null;
            AbilitySelector selector = this;

            while (selector != null)
            {
                result = selector.TrySelectAbility(world, executor, worldPosition);

                if (result != null)
                    break;

                selector = selector.Next;
            }

            return result;
        }

        protected abstract AbilityPlan TrySelectAbility(GameWorld world, EntityModel executor, Vector3 worldPosition);
    }
}
