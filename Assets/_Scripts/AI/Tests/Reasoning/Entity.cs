using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record Entity(bool IsPlayer, int id) : AIUnitModel(IsPlayer, id)
    {
        public int CurrentHealth { get; set; }
        public Vector2 Position { get; set; }

        public override AIUnitModel DeepCopy()
        {
            return new Entity(IsPlayer, Id)
            {
                PossibleActions = PossibleActions.DeepCopy(),
                CurrentHealth = CurrentHealth,
                Position = Position
            };
        }
    }
}
