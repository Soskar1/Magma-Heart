using MagmaHeart.AI.Actions;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal sealed class EntityBuilder
    {
        private readonly AIScenarioBuilder m_scenario;
        private List<ActionData> m_actions;
        private int m_health = 5;
        private bool m_isPlayer = false;
        private bool m_surroundWithWalls = false;

        public EntityBuilder(AIScenarioBuilder scenario) => m_scenario = scenario;

        public EntityBuilder WithActions(List<ActionData> actions)
        {
            m_actions = actions;
            return this;
        }

        public EntityBuilder WithHealth(int health)
        {
            m_health = health;
            return this;
        }

        public EntityBuilder IsPlayer(bool isPlayer)
        {
            m_isPlayer = isPlayer;
            return this;
        }

        public EntityBuilder SurroundWithWalls()
        {
            m_surroundWithWalls = true;
            return this;
        }

        public AIScenarioBuilder At(int x, int y)
        {
            EntityInitializationData data = new EntityInitializationData(new Vector3Int(x, y), m_isPlayer, m_health, m_actions);
            EntityModel model = BoardBuilder.AddEntity(m_scenario.Board, data);

            if (m_surroundWithWalls)
                BoardBuilder.SurroundWithWalls(m_scenario.Board, new Vector2(x, y));

            EnemyCombatController combatController = new EnemyCombatController(model, null);

            m_scenario.RegisterEntity(combatController);

            return m_scenario;
        }
    }
}
