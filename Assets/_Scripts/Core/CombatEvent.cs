using System;
using MagmaHeart.Core.Dungeon;

namespace MagmaHeart.Core
{
    public class CombatEvent
    {
        private Spawner m_spawner;
        private int m_waves;
        private int m_currentWave;

        private int m_currentAmountOfMonsters;

        public Action OnCombatEventEnded;

        public CombatEvent(Spawner spawner, int waves)
        {
            m_spawner = spawner;
            m_waves = waves;
            m_currentWave = 1;
            m_currentAmountOfMonsters = 0;

            m_spawner.SpawnedEnemy += IncrementCurrentAmountOfMonsters;
            m_spawner.EnemyDisabled += DecrementCurrentAmountOfMonsters;
        }

        public void Start(Room room)
        {
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
                if (m_currentWave < m_waves)
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
            OnCombatEventEnded?.Invoke();
        }
    }
}