using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class MovementAction
    {
        private Entity m_entity;
        private Room m_currentRoom;
        private int m_freeDistanceToMove;
        private int m_currentTheoreticalFreeDistanceToMove;

        public int CurrentTheoreticalEnergyUsage { get; private set; } = 0;
        public Vector3Int CurrentTilePosition { get; private set; }
        public int MovementDistanceInTilesForOneEnergy { get; private set; } = 2;

        public MovementAction(Entity entity)
        {
            m_entity = entity;
        }

        public void Reset()
        {
            m_freeDistanceToMove = 0;
            CurrentTheoreticalEnergyUsage = 0;
        }

        public void SetCurrentRoom(Room room) => m_currentRoom = room;

        public bool CanMoveToTile(Vector3Int targetTile)
        {
            CalculateEnergyUsage(targetTile);
            return m_entity.Energy.HasEnough(CurrentTheoreticalEnergyUsage) && m_currentRoom.TileIsAccessable(targetTile);
        }

        public void MoveWithEnergyCost(Vector3Int targetTile)
        {
            if (CanMoveToTile(targetTile))
            {
                m_entity.Energy.Spend(CurrentTheoreticalEnergyUsage);
                Move(targetTile);
                m_freeDistanceToMove = m_currentTheoreticalFreeDistanceToMove;
            }
        }

        public void Move(Vector3Int targetTile)
        {
            m_entity.Transform.position = m_currentRoom.Grid.ToTileCenter(targetTile);
            CurrentTilePosition = targetTile;
        }

        public void CalculateEnergyUsage(Vector3Int targetTile)
        {
            Vector3Int currentTile = m_currentRoom.Grid.WorldToTilePosition(m_entity.Transform.position);
            int distance = m_currentRoom.Grid.ManhattanDistance(currentTile, targetTile) - m_freeDistanceToMove;
            CurrentTheoreticalEnergyUsage = Mathf.CeilToInt(distance / (float)MovementDistanceInTilesForOneEnergy);
            m_currentTheoreticalFreeDistanceToMove = distance % MovementDistanceInTilesForOneEnergy;
        }
    }
}