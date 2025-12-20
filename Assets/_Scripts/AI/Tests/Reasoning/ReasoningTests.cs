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

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Database = new ActionDatabase(assembly);
            Strategy = new BasicStrategy(null);
        }

        [SetUp]
        public void SetUp()
        {
            BoardGraph graph = new BoardGraph();
            for (int i = -10; i < 10; ++i)
                for (int j = -10; j < 10; ++j)
                    graph.AddNode(new Vector2(i, j), BoardNodeType.Walkable);

            Board = new Board(graph);
        }

        public Entity CreateEntity(int health, Vector2 position, bool isPlayer)
        {
            Entity entity = new Entity(health, position, isPlayer);
            AttackActionData attackData = new AttackActionData(4);
            MoveActionData moveData = new MoveActionData(3);
            EngageActionData engageData = new EngageActionData(4, 1);
            RunAwayActionData runAwayData = new RunAwayActionData(3);

            entity.PossibleActions.Add(attackData.GetDefinition());
            entity.PossibleActions.Add(moveData.GetDefinition());
            entity.PossibleActions.Add(engageData.GetDefinition());
            entity.PossibleActions.Add(runAwayData.GetDefinition());
            Board.AddUnit(position, entity);

            return entity;
        }
    }
}
