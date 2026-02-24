using MagmaHeart.Core.Abilities.Presentation;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects.Presenters
{
    public class MoveEffectPresenter : IEffectPresenter<MoveEffect>
    {
        private readonly CombatTilemapPresenter m_combatTilemapPresenter;

        public MoveEffectPresenter(CombatTilemapPresenter combatTilemapPresenter) => m_combatTilemapPresenter = combatTilemapPresenter;

        public void Present(GameWorld world, MoveEffect effect)
        {
            foreach (Vector3 point in effect.Path.Skip(1))
                m_combatTilemapPresenter.DisplayTile(point.ToVector3Int(), true);
        }
    }
}
