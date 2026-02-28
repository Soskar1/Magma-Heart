using MagmaHeart.Abilities;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning
{
    public class WorldSimulation : IBoardGameWorld
    {
        private readonly Board m_board;
        private readonly AStar m_aStar;

        public WorldSimulation(Board board)
        {
            m_board = board;
            m_aStar = new AStar(AStar.ManhattanDistance);
        }

        public void AddUnit(Vector2 position, AIUnitModel unit) => m_board.AddUnit(position, unit);

        public bool AreEnemiesToEachOther(int executorId, int targetId)
        {
            m_board.TryGetUnit(executorId, out AIUnitModel executor);
            m_board.TryGetUnit(targetId, out AIUnitModel target);

            return executor != null && target != null && executor.IsPlayer != target.IsPlayer;
        }

        public void ChangeNodeType(Vector2 position, BoardNodeType newNodeType) => m_board.ChangeNodeType(position, newNodeType);

        public int GetDistance(int entityId1, int entityId2)
        {
            m_board.TryGetUnitPosition(entityId1, out Vector2 position1);
            m_board.TryGetUnitPosition(entityId2, out Vector2 position2);

            return (int)Mathf.Abs(position1.x - position2.x) +
                   (int)Mathf.Abs(position1.y - position2.y);
        }

        public int GetEntityAtPosition(Vector3 position) => m_board.TryGetUnit(position, out AIUnitModel unit) ? unit.Id : -1;

        public Vector3 GetEntityPosition(int entityId) => m_board.TryGetUnitPosition(entityId, out Vector2 position) ? (Vector3)position : Vector3.negativeInfinity;

        public BoardNodeType GetNodeType(Vector2 position) => m_board.GetNodeType(position);

        public IParameter GetParameter(int entityId, ParameterId parameter)
        {
            m_board.TryGetUnit(entityId, out AIUnitModel unit);
            return unit.GetParameter(parameter);
        }

        public AIUnitModel GetUnit(int id)
        {
            throw new System.NotImplementedException();
        }

        public void MoveUnit(int unitId, Vector2 newPosition)
        {
            throw new System.NotImplementedException();
        }

        public bool PositionIsAccessible(Vector3 position) => m_board.GetNodeType(position) == BoardNodeType.Walkable;

        public bool RemoveUnit(Vector2 position) => m_board.RemoveUnit(position);

        public void SetParameter(int entityId, ParameterId parameter, float newValue)
        {
            throw new System.NotImplementedException();
        }

        public bool TryFindPath(Vector3 from, Vector3 to, out List<Vector3> path)
        {
            path = m_aStar
                .FindPath(m_board.Graph, from, to)
                .Select(point => (Vector3)point)
                .ToList();

            if (path == null || path.Count == 0 || path.Last() != to)
                return false;

            return true;
        }

        public Vector2 WorldPositionToBoardTile(Vector2 worldPosition)
        {
            throw new System.NotImplementedException();
        }

        public Vector2 WorldToTilePosition(Vector2 worldPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}