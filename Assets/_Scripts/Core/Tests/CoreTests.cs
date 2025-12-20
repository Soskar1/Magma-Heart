using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal class CoreTests
    {
        private CombatBoardState m_state;
        private const int m_boardDimensions = 10;

        public CombatBoardState State => m_state;

        [SetUp]
        public void SetUp()
        {
            BoardGraph graph = new BoardGraph();
            for (int x = 0; x < m_boardDimensions; ++x)
            {
                for (int y = 0; y < m_boardDimensions; ++y)
                    graph.AddNode(new Vector2(x, y), BoardNodeType.Walkable);

                for (int y = 0; y < m_boardDimensions - 1; ++y)
                    graph.ConnectNodes(new Vector2(x, y), new Vector2(x, y + 1), 1);

                if (x > 0)
                    for (int y = 0; y < m_boardDimensions; ++y)
                        graph.ConnectNodes(new Vector2(x - 1, y), new Vector2(x, y), 1);
            }

            Board board = new Board(graph);
            Room room = new Room(null, null, null, graph);
            m_state = new CombatBoardState(room);
        }

        public TurnContext<EntityModel> AddEntity(Vector3Int position, bool isPlayer, int maxHealth = 5)
        {
            EntityStats stats = new EntityStats(maxHealth);
            List<ActionData> actions = new List<ActionData>
            {
                new AttackActionData(2, 1, 1),
                new MovementActionData(2)
            };
            EntityData data = new EntityData("", stats, actions);
            EntityModel model = new EntityModel(data, () => position, isPlayer);
            m_state.Room.AddUnit(position.ToVector2(), model);
            m_state.Room.ChangeNodeType(position.ToVector2(), BoardNodeType.Obstacle);

            return new EnemyCombatController(model, null);
        }
    }
}
