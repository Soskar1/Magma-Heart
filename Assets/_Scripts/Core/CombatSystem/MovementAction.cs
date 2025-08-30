using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Navigation;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        private List<RoomTile> m_currentPath;

        public int CurrentTheoreticalEnergyUsage { get; private set; } = 0;
        public int MovementDistanceInTilesForOneEnergy { get; private set; } = 2;
        public List<RoomTile> CurrentPath
        {
            get
            {
                if (m_currentPath == null)
                    m_currentPath = new List<RoomTile>();

                return m_currentPath;
            }
            private set
            {
                if (value == null)
                    return;

                m_currentPath = value;
            }
        }
        public RoomTile TileToMove { get; set; }

        public MovementAction(TurnBasedMovement movement, Energy energy, ITilePosition tilePosition)
        {
            m_movement = movement;
            m_energy = energy;
            m_tilePosition = tilePosition;
            m_aStar = new AStar(AStar.ManhattanDistance);
            CurrentPath = new List<RoomTile>();
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

            int distance = CurrentPath.Count - 1 - m_freeDistanceToMove;
            CurrentTheoreticalEnergyUsage = Mathf.CeilToInt(distance / (float)MovementDistanceInTilesForOneEnergy);
            m_currentTheoreticalFreeDistanceToMove = distance % MovementDistanceInTilesForOneEnergy;

            return m_energy.HasEnough(CurrentTheoreticalEnergyUsage);
        }

        private void Move()
        {
            if (CurrentPath.Count == 0)
                return;

            m_movement.StartMovement(CurrentPath);

            OnMovedEventArgs args = new OnMovedEventArgs(CurrentPath.First().Position, CurrentPath.Last().Position);
            m_tilePosition.OnMoved?.Invoke(this, args);
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
            CurrentPath = path.Select(v => m_currentRoom.GetRoomTile(v)).ToList();
        }
    }
}