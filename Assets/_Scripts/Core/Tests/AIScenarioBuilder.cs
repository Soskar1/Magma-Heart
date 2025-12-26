using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using MagmaHeart.AI.Boards;

namespace MagmaHeart.Core.Tests
{
    internal sealed class AIScenarioBuilder
    {
        private readonly CombatBoardState m_state;
        public Board Board => m_state.Board;

        private readonly List<TurnContext<EntityModel>> m_entities = new();
        private readonly List<TurnContext<EntityModel>> m_players = new();
        private readonly List<TurnContext> m_turnOrder = new();

        private AIScenarioBuilder(CombatBoardState state) => m_state = state;

        public static AIScenarioBuilder Create(CombatBoardState state) => new AIScenarioBuilder(state);
        public EntityBuilder AddEntity() => new EntityBuilder(this);
        public BoardBuilder ModifyBoard() => new BoardBuilder(this);
        internal void RegisterEntity(TurnContext<EntityModel> entity)
        {
            m_entities.Add(entity);

            if (entity.Model.IsPlayer)
                m_players.Add(entity);

            m_turnOrder.Add(entity);
        }

        public AIScenario Build()
        {
            return new AIScenario(m_state, new TurnOrder(m_turnOrder));
        }
    }
}