using System.Collections.Generic;
using MagmaHeart.Abilities;
using MagmaHeart.AI.Boards;
using MagmaHeart.Core.Entities;
using MagmaHeart.Extensions;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal sealed class EntityBuilder
    {
        private readonly AIScenarioBuilder m_scenario;
        private readonly int m_id;
        private bool m_isPlayer = false;
        private EntityData m_data;
        private IDictionary<ParameterId, float> m_parameterValues = new Dictionary<ParameterId, float>();

        public EntityBuilder(AIScenarioBuilder scenario, int id)
        {
            m_scenario = scenario;
            m_id = id;
        }

        public EntityBuilder IsPlayer(bool isPlayer)
        {
            m_isPlayer = isPlayer;
            return this;
        }

        public EntityBuilder WithData(EntityData data)
        {
            m_data = data;
            return this;
        }

        public EntityBuilder WithParameterValue(ParameterId parameter, float value)
        {
            m_parameterValues.Add(parameter, value);
            return this;
        }

        public AIScenarioBuilder At(int x, int y)
        {
            Vector2 position = new Vector2(x, y);
            EntityModel model = new EntityModel(m_data, position.ToVector3Int(), m_isPlayer, m_id);

            foreach (var parameter in m_parameterValues.Keys)
                model.GetParameter(parameter).SetValue(m_parameterValues[parameter]);

            m_scenario.World.AddUnit(position, model);
            m_scenario.World.ChangeNodeType(position, BoardNodeType.Obstacle);
            m_scenario.RegisterEntity(model);

            return m_scenario;
        }
    }
}
