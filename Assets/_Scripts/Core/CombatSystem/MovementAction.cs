using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class MovementAction
    {
        private ITilePosition m_tilePosition;
        private Energy m_energy;
        private Room m_currentRoom;
        private int m_freeDistanceToMove;
        private int m_currentTheoreticalFreeDistanceToMove;

        public int CurrentTheoreticalEnergyUsage { get; private set; } = 0;
        public int MovementDistanceInTilesForOneEnergy { get; private set; } = 2;

        public MovementAction(Energy energy, ITilePosition tilePosition)
        {
            m_energy = energy;
            m_tilePosition = tilePosition;
        }

        public void Reset()
        {
            m_freeDistanceToMove = 0;
            CurrentTheoreticalEnergyUsage = 0;
        }

        public void SetCurrentRoom(Room room) => m_currentRoom = room;

        public bool CanMoveToTile(RoomTile targetTile)
        {
            CalculateEnergyUsage(targetTile);
            return m_energy.HasEnough(CurrentTheoreticalEnergyUsage) && m_currentRoom.TileIsAccessable(targetTile);
        }

        public void MoveWithEnergyCost(RoomTile targetTile)
        {
            if (CanMoveToTile(targetTile))
            {
                m_energy.Spend(CurrentTheoreticalEnergyUsage);
                Move(targetTile);
                m_freeDistanceToMove = m_currentTheoreticalFreeDistanceToMove;
            }
        }

        public void Move(RoomTile targetTile)
        {
            m_tilePosition.Transform.position = m_currentRoom.Grid.ToTileCenter(targetTile.Position);
            m_tilePosition.CurrentTilePosition = targetTile.Position;
        }

        private void CalculateEnergyUsage(RoomTile targetTile)
        {
            Vector3Int currentTile = m_currentRoom.Grid.WorldToTilePosition(m_tilePosition.Transform.position);
            int distance = DungeonGrid.ManhattanDistance(currentTile, targetTile.Position) - m_freeDistanceToMove;
            CurrentTheoreticalEnergyUsage = Mathf.CeilToInt(distance / (float)MovementDistanceInTilesForOneEnergy);
            m_currentTheoreticalFreeDistanceToMove = distance % MovementDistanceInTilesForOneEnergy;
        }
    }
}