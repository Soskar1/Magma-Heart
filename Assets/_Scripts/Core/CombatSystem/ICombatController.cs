using System;
using MagmaHeart.Core.Dungeon;

namespace MagmaHeart.Core.CombatSystem
{
    public interface ICombatController : IHittableTile
    {
        public bool IsPlayableCharacter { get; }
        public Action NextTurn { get; set; }
        public void StartTurn();
        public void EndTurn();
        public void StartCombat(Room room);
    }
}