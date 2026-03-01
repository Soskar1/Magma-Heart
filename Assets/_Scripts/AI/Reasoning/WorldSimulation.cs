using MagmaHeart.Abilities;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning
{
    public class WorldSimulation : IBoardGameWorld
    {
        private readonly Board m_board;
        private readonly AStar m_aStar;

        private readonly Stack<Action> m_undoStack = new Stack<Action>();
        private readonly Stack<int> m_checkpoints = new Stack<int>();

        public WorldSimulation(Board board)
        {
            m_board = board;
            m_aStar = new AStar(AStar.ManhattanDistance);
        }

        public void SaveCheckpoint() => m_checkpoints.Push(m_undoStack.Count);

        public void RestoreCheckpoint()
        {
            int targetCount = 0;

            if (m_checkpoints.Count > 0)
                targetCount = m_checkpoints.Pop();

            while (m_undoStack.Count > targetCount)
                m_undoStack.Pop().Invoke();
        }

        public void AddUnit(Vector2 position, AIUnitModel unit)
        {
            m_undoStack.Push(() => m_board.RemoveUnit(position));
            m_board.AddUnit(position, unit);
        }

        public bool RemoveUnit(Vector2 position)
        {
            if (m_board.TryGetUnit(position, out AIUnitModel unit))
            {
                m_undoStack.Push(() => m_board.AddUnit(position, unit));
                return m_board.RemoveUnit(position);
            }
            return false;
        }

        public void ChangeNodeType(Vector2 position, BoardNodeType newNodeType)
        {
            BoardNodeType oldType = m_board.GetNodeType(position);
            m_undoStack.Push(() => m_board.ChangeNodeType(position, oldType));
            m_board.ChangeNodeType(position, newNodeType);
        }

        public void MoveUnit(int unitId, Vector2 newPosition)
        {
            if (m_board.TryGetUnit(unitId, out AIUnitModel unit))
            {
                RemoveUnit(GetEntityPosition(unitId));
                AddUnit(newPosition, unit);
            }
        }

        public void SetParameter(int entityId, ParameterId parameterId, float newValue)
        {
            IParameter parameter = GetParameter(entityId, parameterId);
            if (parameter != null)
            {
                float oldValue = parameter.CurrentValue;
                m_undoStack.Push(() => parameter.SetValue(oldValue));
                parameter.SetValue(newValue);
            }
        }

        public AIUnitModel GetUnit(int id)
        {
            m_board.TryGetUnit(id, out AIUnitModel unit);
            return unit;
        }

        public bool AreEnemiesToEachOther(int executorId, int targetId)
        {
            m_board.TryGetUnit(executorId, out AIUnitModel executor);
            m_board.TryGetUnit(targetId, out AIUnitModel target);

            return executor != null && target != null && executor.IsPlayer != target.IsPlayer;
        }

        public int GetDistance(int entityId1, int entityId2)
        {
            Vector3 position1 = GetEntityPosition(entityId1);
            Vector3 position2 = GetEntityPosition(entityId2);

            return (int)Mathf.Abs(position1.x - position2.x) +
                   (int)Mathf.Abs(position1.y - position2.y);
        }

        public Vector3 GetEntityPosition(int entityId) => m_board.TryGetUnitPosition(entityId, out Vector2 position) ? (Vector3)position : Vector3.negativeInfinity;

        public BoardNodeType GetNodeType(Vector2 position) => m_board.GetNodeType(position);

        public IParameter GetParameter(int entityId, ParameterId parameter)
        {
            m_board.TryGetUnit(entityId, out AIUnitModel unit);
            return unit.GetParameter(parameter);
        }

        public bool TryFindPath(Vector3 from, Vector3 to, out List<Vector3> path)
        {
            path = null;
            var tmpPath = m_aStar.FindPath(m_board.Graph, from, to);
                
            if (tmpPath == null || tmpPath.Count == 0 || tmpPath.Last() != (Vector2)to)
                return false;

            path = tmpPath
                .Select(point => (Vector3)point)
                .ToList();

            return true;
        }

        public Vector2 WorldToTilePosition(Vector2 worldPosition) => worldPosition;

        public IEnumerable<AIUnitModel> GetUnits() => m_board.GetUnits();
    }
}