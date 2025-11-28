using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.Presenters;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class Enemy : EntityPresenter
    {
        public void Initialize(DungeonGrid grid, CombatAI ai)
        {
            base.Initialize(grid, false);

            TurnContext = new EnemyCombatController(Model, ai);
        }

        private void Update() => Animation.PlayAnimations();
    }
}