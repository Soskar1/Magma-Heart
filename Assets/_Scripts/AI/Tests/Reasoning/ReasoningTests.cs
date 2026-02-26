using MagmaHeart.AI.Boards;
using NUnit.Framework;
using System.Reflection;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class ReasoningTests
    {
        public Board Board { get; private set; }
        public BasicStrategy Strategy { get; private set; }
        private int m_nextId = 0;

        [OneTimeSetUp]
        public void InitializeDatabase()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Strategy = new BasicStrategy();
        }

        [SetUp]
        public void InitializeBoard()
        {
            BoardGraph graph = new BoardGraph();
            for (int i = -10; i < 10; ++i)
                for (int j = -10; j < 10; ++j)
                    graph.AddNode(new Vector2(i, j), BoardNodeType.Walkable);

            Board = new Board(graph);
        }

        public Entity CreateEntity(int health, Vector2 position, bool isPlayer)
        {
            Entity entity = new Entity(isPlayer, m_nextId);
            entity.CurrentHealth = health;
            entity.Position = position;
            ++m_nextId;
            
            Board.AddUnit(position, entity);

            return entity;
        }
    }
}
