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
        private ITilePosition m_tilePosition;
        private Energy m_energy;
        private Room m_currentRoom;
        private AStar m_aStar;
        private int m_freeDistanceToMove;
        private int m_currentTheoreticalFreeDistanceToMove;

        public int CurrentTheoreticalEnergyUsage { get; private set; } = 0;
        public int MovementDistanceInTilesForOneEnergy { get; private set; } = 2;
        public List<Vector2> CurrentPath { get; private set; }
        public RoomTile TileToMove { get; set; }

        public MovementAction(Energy energy, ITilePosition tilePosition)
        {
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
                Move(TileToMove);
                m_freeDistanceToMove = m_currentTheoreticalFreeDistanceToMove;
            }
        }

        public bool CanMoveToTile(RoomTile targetTile)
        {
            // TODO: implement cache

            if (!m_currentRoom.TileIsAccessable(targetTile))
                return false;

            Vector3Int currentTile = m_currentRoom.Grid.WorldToTilePosition(m_tilePosition.Transform.position);
            List<Vector2> path = m_aStar.FindPath(m_currentRoom.AStarGraph, currentTile.ToVector2(), targetTile.Position.ToVector2());
            
            if (path == null)
                return false;

            CurrentPath = path;

            int distance = CurrentPath.Count - m_freeDistanceToMove;
            CurrentTheoreticalEnergyUsage = Mathf.CeilToInt(distance / (float)MovementDistanceInTilesForOneEnergy);
            m_currentTheoreticalFreeDistanceToMove = distance % MovementDistanceInTilesForOneEnergy;

            return m_energy.HasEnough(CurrentTheoreticalEnergyUsage);
        }

        public void Move(RoomTile targetTile)
        {
            m_tilePosition.Transform.position = m_currentRoom.Grid.ToTileCenter(targetTile.Position);
            m_tilePosition.CurrentTilePosition = targetTile.Position;
        }
    }
}