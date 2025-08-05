using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class MovementAction
    {
        private Entity m_entity;
        private Room m_currentRoom;

        public int MovementDistanceInTilesForOneEnergy { get; private set; } = 2;

        public MovementAction(Entity entity)
        {
            m_entity = entity;
        }

        public void SetCurrentRoom(Room room) => m_currentRoom = room;

        public void TryMove(Vector3Int targetTile)
        {
            int energyCost = CalculateEnergyUsage(targetTile);

            if (m_entity.Energy.HasEnough(energyCost))
            {
                m_entity.Energy.Spend(energyCost);
                m_entity.Transform.position = m_currentRoom.Grid.ToTileCenter(targetTile);
            }
        }

        public int CalculateEnergyUsage(Vector3Int targetTile)
        {
            Vector3Int currentTile = m_currentRoom.Grid.WorldToTilePosition(m_entity.Transform.position);
            int distance = m_currentRoom.Grid.ManhattanDistance(currentTile, targetTile);

            return Mathf.CeilToInt(distance / (float)MovementDistanceInTilesForOneEnergy);
        }
    }
}