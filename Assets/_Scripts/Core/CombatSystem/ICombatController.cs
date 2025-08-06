using System;
using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public interface ICombatController
    {
        public Vector3Int CurrentTilePosition { get; }
        public bool IsPlayableCharacter { get; }
        public Action NextTurn { get; set; }
        public void StartTurn();
        public void EndTurn();
        public void StartCombat(Room room);
    }
}