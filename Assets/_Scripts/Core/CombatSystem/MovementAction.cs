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

        public int MovementDistanceInTilesForOneEnergy { get; private set; } = 2;

        public MovementAction(Entity entity)
        {
            m_entity = entity;
        }
        public void Reset() => m_freeDistanceToMove = 0;

        public void SetCurrentRoom(Room room) => m_currentRoom = room;

        public void TryMove(Vector3Int targetTile)
        {
            (int energyCost, int reminder) = CalculateEnergyUsage(targetTile);

            if (m_entity.Energy.HasEnough(energyCost))
            {
                m_entity.Energy.Spend(energyCost);
                m_entity.Transform.position = m_currentRoom.Grid.ToTileCenter(targetTile);
                m_freeDistanceToMove = reminder;
            }
        }

        public (int, int) CalculateEnergyUsage(Vector3Int targetTile)
        {
            Vector3Int currentTile = m_currentRoom.Grid.WorldToTilePosition(m_entity.Transform.position);
            int distance = m_currentRoom.Grid.ManhattanDistance(currentTile, targetTile) - m_freeDistanceToMove;
            int energyCost = Mathf.CeilToInt(distance / (float)MovementDistanceInTilesForOneEnergy);
            int reminder = distance % MovementDistanceInTilesForOneEnergy;

            return (energyCost, reminder);
        }
    }
}