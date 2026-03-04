using MagmaHeart.Abilities;
using MagmaHeart.AI.Boards;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI
{
    public interface IBoardGameWorld : IGameWorld
    {
        public void ChangeNodeType(Vector2 position, BoardNodeType newNodeType);
        public BoardNodeType GetNodeType(Vector2 position);
        public void AddUnit(Vector2 position, AIUnitModel unit);
        public AIUnitModel GetUnit(int id);
        public bool RemoveUnit(Vector2 position);
        public void MoveUnit(int unitId, Vector2 newPosition);
        public Vector2 WorldToTilePosition(Vector2 worldPosition);
        public IEnumerable<AIUnitModel> GetUnits();
    } 
}
