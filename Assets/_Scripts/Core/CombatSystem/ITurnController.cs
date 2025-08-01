using System;

namespace MagmaHeart.Core.CombatSystem
{
    public interface ITurnController
    {
        public Action NextTurn { get; set; }
        public void StartTurn();
        public void EndTurn();
    }
}