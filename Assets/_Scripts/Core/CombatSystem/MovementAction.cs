using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Navigation;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class MovementAction : ICombatAction
    {
        private TurnBasedMovement m_movement;
        private ITilePosition m_tilePosition;
        private Energy m_energy;
        private Room m_currentRoom;
        private AStar m_aStar;
        private int m_freeDistanceToMove;
        private int m_currentTheoreticalFreeDistanceToMove;
        private List<Vector2> m_currentPath;

        public int CurrentTheoreticalEnergyUsage { get; private set; } = 0;
        public int MovementDistanceInTilesForOneEnergy { get; private set; } = 2;
        public List<Vector2> CurrentPath
        {
            get
            {
                if (m_currentPath == null)
                    m_currentPath = new List<Vector2>();

                return m_currentPath;
            }
            private set
            {
                if (value == null)
                    return;

                m_currentPath = value;

                for (int i = 0; i < m_currentPath.Count; i++)
                    m_currentPath[i] = m_currentRoom.Grid.ToTileCenter(m_currentPath[i].ToVector2Int());
            }
        }
        public RoomTile TileToMove { get; set; }

        public MovementAction(TurnBasedMovement movement, Energy energy, ITilePosition tilePosition)
        {
            m_movement = movement;
            m_energy = energy;
            m_tilePosition = tilePosition;
            m_aStar = new AStar(AStar.ManhattanDistance);
            CurrentPath = new List<Vector2>();
        }

        public void Reset()
        {
            m_freeDistanceToMove = 0;
            CurrentTheoreticalEnergyUsage = 0;
            CurrentPath.Clear();
        }

        public void SetCurrentRoom(Room room) => m_currentRoom = room;

        public void Execute()
        {
            if (CanMoveToTile(TileToMove))
            {
                m_energy.Spend(CurrentTheoreticalEnergyUsage);
                Move();
                m_freeDistanceToMove = m_currentTheoreticalFreeDistanceToMove;
            }
        }

        public bool CanMoveToTile(RoomTile targetTile)
        {
            // TODO: implement cache

            if (!m_currentRoom.TileIsAccessable(targetTile))
                return false;

            CalculatePath(targetTile);

            int distance = CurrentPath.Count - m_freeDistanceToMove;
            CurrentTheoreticalEnergyUsage = Mathf.CeilToInt(distance / (float)MovementDistanceInTilesForOneEnergy);
            m_currentTheoreticalFreeDistanceToMove = distance % MovementDistanceInTilesForOneEnergy;

            return m_energy.HasEnough(CurrentTheoreticalEnergyUsage);
        }

        public void Move()
        {
            if (CurrentPath.Count == 0)
                return;

            m_movement.StartMovement(CurrentPath);
        }

        public void MoveWithoutEnergyUsage(RoomTile targetTile)
        {
            CalculatePath(targetTile);
            Move();
        }

        private void CalculatePath(RoomTile targetTile)
        {
            Vector3Int currentTile = m_tilePosition.CurrentTilePosition;
            List<Vector2> path = m_aStar.FindPath(m_currentRoom.AStarGraph, currentTile.ToVector2(), targetTile.Position.ToVector2());
            CurrentPath = path;
        }
    }
}