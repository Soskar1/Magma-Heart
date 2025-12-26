using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Extensions;
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

        public AIScenarioBuilder At(int x, int y)
        {
            Vector2 position = new Vector2(x, y);
            EntityStats stats = new EntityStats(m_health);
            EntityData data = new EntityData("", stats, m_actions == null ? new List<ActionData>() : m_actions);
            EntityModel model = new EntityModel(data, () => position.ToVector3Int(), m_isPlayer);
            m_scenario.Board.AddUnit(position, model);
            m_scenario.Board.ChangeNodeType(position, BoardNodeType.Obstacle);

            EnemyCombatController combatController = new EnemyCombatController(model, null);

            m_scenario.RegisterEntity(combatController);

            return m_scenario;
        }
    }
}
