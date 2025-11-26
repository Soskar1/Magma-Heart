using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.NonPlayableCharacters;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class Enemy : EntityPresenter
    {
        public void Initialize(DungeonGrid grid, CombatAI ai)
        {
            base.Initialize(grid, false);

            CombatController = new EnemyCombatController(Model, ai);
        }

        private void Update() => Animation.PlayAnimations();
    }
}