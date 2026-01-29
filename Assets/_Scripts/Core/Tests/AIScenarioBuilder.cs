using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using MagmaHeart.AI.Boards;
using MagmaHeart.Collections;
using MagmaHeart.AI;

namespace MagmaHeart.Core.Tests
{
    internal sealed class AIScenarioBuilder
    {
        private readonly Board m_board;
        public Board Board => m_board;

        private readonly List<EntityModel> m_entities = new();
        private readonly List<EntityModel> m_players = new();
        private readonly CircularList<AIUnitModel> m_turnOrder = new();

        private int m_nextId = 0;

        private AIScenarioBuilder(Board state) => m_board = state;

        public static AIScenarioBuilder Create(Board board) => new AIScenarioBuilder(board);
        
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
            return new AIScenario(m_board, m_turnOrder);
        }
    }
}