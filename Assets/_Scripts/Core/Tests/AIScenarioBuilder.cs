using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using System;
using MagmaHeart.AI.Boards;
using System.Linq;

namespace MagmaHeart.Core.Tests
{
    internal sealed class AIScenarioBuilder
    {
        private readonly CombatBoardState m_state;
        public Board Board => m_state.Board;

        private readonly List<TurnContext<EntityModel>> m_entities = new();
        private readonly List<TurnContext<EntityModel>> m_players = new();
        private readonly List<TurnContext<EntityModel>> m_enemies = new();
        private readonly List<TurnContext> m_turnOrder = new();

        private AIScenarioBuilder(CombatBoardState state) => m_state = state;

        public static AIScenarioBuilder Create(CombatBoardState state) => new AIScenarioBuilder(state);
        public EntityBuilder AddEntity() => new EntityBuilder(this);
        internal void RegisterEntity(TurnContext<EntityModel> entity)
        {
            m_entities.Add(entity);

            if (entity.Model.IsPlayer)
                m_players.Add(entity);
            else
                m_enemies.Add(entity);

            m_turnOrder.Add(entity);
        }

        public AIScenario Build()
        {
            if (m_entities.Count == 0 || m_enemies.Count == 0 || m_players.Count != 1)
                throw new InvalidOperationException("Scenario must contain 1 player and enemies (>0)");

            return new AIScenario(m_state, new TurnOrder(m_turnOrder), m_players.First().TypedModel);
        }
    }
}