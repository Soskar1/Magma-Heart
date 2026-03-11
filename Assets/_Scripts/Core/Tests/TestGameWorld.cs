using System.Collections.Generic;
using System.Linq;
using MagmaHeart.Abilities;
using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Pathfinding;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    public class TestGameWorld : IBoardGameWorld
    {
        public Board Board { get; init; }

        private readonly AStar m_aStar;

        public TestGameWorld(Board board)
        {
            Board = board;
            m_aStar = new AStar(AStar.ManhattanDistance);
        }

        public void AddUnit(Vector2 position, AIUnitModel unit)
        {
            Board.AddUnit(position, unit);
        }

        public bool AreEnemiesToEachOther(int executorId, int targetId)
        {
            Board.TryGetUnit(executorId, out AIUnitModel executor);
            Board.TryGetUnit(targetId, out AIUnitModel target);

            return executor != null && target != null && executor.IsPlayer != target.IsPlayer;
        }

        public void ChangeNodeType(Vector2 position, BoardNodeType newNodeType)
        {
            Board.ChangeNodeType(position, newNodeType);
        }

        public int GetDistance(int entityId1, int entityId2)
        {
            Vector3 position1 = GetEntityPosition(entityId1);
            Vector3 position2 = GetEntityPosition(entityId2);

            return (int)Mathf.Abs(position1.x - position2.x) +
                   (int)Mathf.Abs(position1.y - position2.y);
        }

        public Vector3 GetEntityPosition(int entityId) => Board.TryGetUnitPosition(entityId, out Vector2 position) ? (Vector3)position : Vector3.negativeInfinity;

        public BoardNodeType GetNodeType(Vector2 position) => Board.GetNodeType(position);

        public IParameter GetParameter(int entityId, ParameterId parameter)
        {
            Board.TryGetUnit(entityId, out var unit);
            return unit.GetParameter(parameter);
        }

        public AIUnitModel GetUnit(int id)
        {
            Board.TryGetUnit(id, out AIUnitModel unit);
            return unit;
        }

        public IEnumerable<AIUnitModel> GetUnits() => Board.GetUnits();

        public void MoveUnit(int unitId, Vector2 newPosition)
        {
            if (Board.TryGetUnit(unitId, out AIUnitModel unit))
            {
                RemoveUnit(GetEntityPosition(unitId));
                AddUnit(newPosition, unit);
            }
        }

        public bool RemoveUnit(Vector2 position) => Board.RemoveUnit(position);

        public void SetParameter(int entityId, ParameterId parameter, float newValue)
        {
            IParameter p = GetParameter(entityId, parameter);
            p?.SetValue(newValue);
        }

        public bool TryFindPath(Vector3 from, Vector3 to, out List<Vector3> path)
        {
            path = null;
            var tmpPath = m_aStar.FindPath(Board.Graph, from, to);

            if (tmpPath == null || tmpPath.Count == 0 || tmpPath.Last() != (Vector2)to)
                return false;

            path = tmpPath
                .Select(point => (Vector3)point)
                .ToList();

            return true;
        }

        public Vector2 WorldToTilePosition(Vector2 worldPosition) => worldPosition;

        public void SetCooldown(int unitId, string abilityId, int turns)
        {
            AIUnitModel unit = GetUnit(unitId);
            unit.SetCooldown(abilityId, turns);
        }

        public int GetCooldown(int entityId, string abilityId)
        {
            AIUnitModel unit = GetUnit(entityId);
            return unit.GetCooldown(abilityId);
        }
    }
}
