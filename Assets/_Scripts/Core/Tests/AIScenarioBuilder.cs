using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using MagmaHeart.Collections;
using MagmaHeart.AI;
using MagmaHeart.Core.Abilities;

namespace MagmaHeart.Core.Tests
{
    internal sealed class AIScenarioBuilder
    {
        private readonly TestGameWorld m_world;
        public TestGameWorld World => m_world;

        private readonly ParameterDatabase m_database;
        public ParameterDatabase Database => m_database;

        private readonly List<EntityModel> m_entities = new();
        private readonly List<EntityModel> m_players = new();
        private readonly CircularList<AIUnitModel> m_turnOrder = new();

        private int m_nextId = 0;

        private AIScenarioBuilder(TestGameWorld world, ParameterDatabase database = null)
        {
            m_world = world;
            m_database = database;
        }

        public static AIScenarioBuilder Create(TestGameWorld world, ParameterDatabase database = null) => new AIScenarioBuilder(world, database);
        
        public EntityBuilder AddEntity()
        {
            ++m_nextId;
            return new EntityBuilder(this, m_nextId);
        }

        public BoardBuilder ModifyBoard() => new BoardBuilder(this);
        internal void RegisterEntity(EntityModel entity)
        {
            m_entities.Add(entity);

            if (entity.IsPlayer)
                m_players.Add(entity);

            m_turnOrder.Add(entity);
        }

        public AIScenario Build()
        {
            return new AIScenario(m_world, m_turnOrder);
        }
    }
}