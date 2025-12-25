using MagmaHeart.Core.Dungeon;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class Enemy : Entity
    {
        public void Initialize(DungeonGrid grid, CombatAI ai)
        {
            base.Initialize(grid, false);

            TurnContext = new EnemyCombatController(Model, ai);
        }

        private void Update() => Animation.PlayAnimations();
    }
}