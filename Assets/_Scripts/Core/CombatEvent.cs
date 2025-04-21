using System;
using System.Collections.Generic;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.Dungeon;
using UnityEngine;
using Random=UnityEngine.Random;

namespace MagmaHeart.Core
{
    public class CombatEvent
    {
        private Spawner m_spawner;
        private int m_waves;
        private int m_currentWave;

        public Action OnCombatEventEnded;
        private Room m_currentRoom;
        private int m_currentAmountOfMonsters;
        private List<Artifact> m_possibleRewards;

        public CombatEvent(Spawner spawner, int waves, List<Artifact> rewards)
        {
            m_spawner = spawner;
            m_waves = waves;
            m_currentWave = 1;
            m_currentAmountOfMonsters = 0;

            m_possibleRewards = rewards;

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
            SpawnAward();
        }

        private void SpawnAward()
        {
            Artifact artifactPrefab = m_possibleRewards[Random.Range(0, m_possibleRewards.Count)];
            GameObject.Instantiate(artifactPrefab, m_currentRoom.WorldPosition.ToVector2(), Quaternion.identity);
        }
    }
}