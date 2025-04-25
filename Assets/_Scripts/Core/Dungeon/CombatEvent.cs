using System;
using System.Collections.Generic;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Dungeon
{
    public class CombatEvent
    {
        private Spawner m_spawner;
        private int m_currentWave;

        public Action OnCombatEventEnded;
        private Room m_currentRoom;
        private int m_currentAmountOfMonsters;

        public CombatEvent(Spawner spawner)
        {
            m_spawner = spawner;
            m_currentWave = 1;
            m_currentAmountOfMonsters = 0;

            m_spawner.SpawnedEnemy += IncrementCurrentAmountOfMonsters;
            m_spawner.EnemyDisabled += DecrementCurrentAmountOfMonsters;
        }

        public void Start(Room room)
        {
            m_currentRoom = room;
            m_spawner.SetRoom(room);
            SpawnWave();
            room.playerEnteredRoom -= Start;
        }

        private void IncrementCurrentAmountOfMonsters() => ++m_currentAmountOfMonsters;

        private void DecrementCurrentAmountOfMonsters()
        {
            --m_currentAmountOfMonsters;

            if (m_currentAmountOfMonsters == 0)
            {
                if (m_currentWave < m_currentRoom.CombatData.waves)
                {
                    ++m_currentWave;
                    SpawnWave();
                }
                else
                {
                    EndCombatEvent();
                }
            }
        }

        private void SpawnWave() => m_spawner.SpawnWave();

        private void EndCombatEvent()
        {
            m_currentWave = 1;
            m_currentRoom.CombatData.OnCombatEnded?.Invoke(m_currentRoom);
        }
    }

    [Serializable]
    public class CombatData
    {
        public List<EnemyMeleeBehaviour> prefabs;
        public int enemyCount;
        public int waves;
        public Action<Room> OnCombatEnded;
    }
}