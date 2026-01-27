using MagmaHeart.AI.Actions;
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
        public ActionDatabase Database { get; private set; }
        private int m_nextId = 0;

        [OneTimeSetUp]
        public void InitializeDatabase()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Database = new ActionDatabase(assembly);
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
            Entity entity = new Entity(health, position, isPlayer, m_nextId);
            ++m_nextId;
            
            AttackActionData attackData = new AttackActionData(4);
            MoveActionData moveData = new MoveActionData(3);
            EngageActionData engageData = new EngageActionData(4, 1);
            RunAwayActionData runAwayData = new RunAwayActionData(3);

            entity.PossibleActions.Add(attackData);
            entity.PossibleActions.Add(moveData);
            entity.PossibleActions.Add(engageData);
            entity.PossibleActions.Add(runAwayData);
            Board.AddUnit(position, entity);

            return entity;
        }
    }
}
