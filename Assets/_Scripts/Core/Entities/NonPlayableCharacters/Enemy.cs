using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.CombatSystem;
using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class Enemy : Entity
    {
        public void Initialize(DungeonGrid grid, CombatAI ai)
        {
            base.Initialize(grid, false);

            CombatController = new EnemyCombatController(this, ai);
        }

        private void Update() => Animation.PlayAnimations();
    }
}