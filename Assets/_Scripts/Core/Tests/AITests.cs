using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.States;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    public class AITests
    {
        private CombatBoardState m_state;
        private const int m_boardDimensions = 10;

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

        private TurnContext AddEntity(Vector3Int position, bool isPlayer, int maxHealth = 5)
        {
            EntityStats stats = new EntityStats(maxHealth);
            EntityModel model = new EntityModel(null, stats, () => position, isPlayer);
            m_state.Board.AddUnit(position.ToVector2(), model);
            m_state.Board.ChangeNodeType(position.ToVector2(), BoardNodeType.Obstacle);

            return new EnemyCombatController(model, null);
        }

        private async Task<TacticianAI> StartTurn(TurnOrder turnOrder, int lookAhead, AIUnit player)
        {
            AggressiveStrategy strategy = new AggressiveStrategy(lookAhead, player);
            TacticianAI ai = new TacticianAI(strategy);
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            await turnOrder.Current.StartTurnAsync(m_state, tokenSource.Token);

            return ai;
        }

        private void CreateWall(Vector2 position) => m_state.Board.ChangeNodeType(position, BoardNodeType.Obstacle);

        private void SurroundEntityWithWalls(EntityModel model)
        {
            Vector3Int position = model.GetCurrentTilePosition();

            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    if (x == 0 && y == 0)
                        continue;

                    Vector3Int wallPosition = new Vector3Int(position.x + x, position.y + y);
                    CreateWall(wallPosition.ToVector2());
                }
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task MovementAction_AggressiveStrategy_MovesTowardsPlayer(int depth)
        {
            Vector3Int enemyPosition = new Vector3Int(4, 3);
            TurnContext player = AddEntity(new Vector3Int(2, 3), true);
            TurnContext enemy = AddEntity(enemyPosition, false);
            TurnOrder turnOrder = new TurnOrder(new List<TurnContext>() { enemy, player });
            TacticianAI ai = await StartTurn(turnOrder, depth, player.Owner);

            BestAction best = ai.ChooseBestMove(turnOrder.ToChainNode(), m_state);

            Assert.That(best.Action, Is.TypeOf<MovementAction>());
            Assert.That(best.Args, Is.EqualTo(new MovementActionArgs(enemyPosition.ToVector2(), new Vector2(3, 3))));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task AttackAction_AggressiveStrategy_AttacksPlayer(int depth)
        {
            TurnContext player = AddEntity(new Vector3Int(2, 3), true);
            TurnContext enemy = AddEntity(new Vector3Int(3, 3), false);
            TurnOrder turnOrder = new TurnOrder(new List<TurnContext>() { enemy, player });
            TacticianAI ai = await StartTurn(turnOrder, depth, player.Owner);

            BestAction best = ai.ChooseBestMove(turnOrder.ToChainNode(), m_state);

            Assert.That(best.Action, Is.TypeOf<AttackAction>());
            Assert.That(best.Args, Is.EqualTo(new AttackActionArgs((EntityModel)player.Owner)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task AttackAction_EnemyLowHPAggressiveStrategy_AttacksPlayer(int depth)
        {
            TurnContext player = AddEntity(new Vector3Int(2, 3), true);
            TurnContext enemy = AddEntity(new Vector3Int(3, 3), false, 1);
            TurnOrder turnOrder = new TurnOrder(new List<TurnContext>() { enemy, player });
            TacticianAI ai = await StartTurn(turnOrder, depth, player.Owner);

            BestAction best = ai.ChooseBestMove(turnOrder.ToChainNode(), m_state);

            Assert.That(best.Action, Is.TypeOf<AttackAction>());
            Assert.That(best.Args, Is.EqualTo(new AttackActionArgs((EntityModel)player.Owner)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task DoNothingAction_PlayerSurroundedByWalls_EnemyDoesNothing(int depth)
        {
            TurnContext player = AddEntity(new Vector3Int(2, 3), true);
            SurroundEntityWithWalls((EntityModel)player.Owner);
            TurnContext enemy = AddEntity(new Vector3Int(4, 3), false, 1);
            TurnOrder turnOrder = new TurnOrder(new List<TurnContext>() { enemy, player });
            TacticianAI ai = await StartTurn(turnOrder, depth, player.Owner);

            BestAction best = ai.ChooseBestMove(turnOrder.ToChainNode(), m_state);

            Assert.That(best.Action, Is.TypeOf<DoNothingAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task DoNothingAction_EnemySurroundedByWalls_EnemyDoesNothing(int depth)
        {
            TurnContext player = AddEntity(new Vector3Int(2, 3), true);
            TurnContext enemy = AddEntity(new Vector3Int(4, 3), false, 1);
            SurroundEntityWithWalls((EntityModel)enemy.Owner);
            TurnOrder turnOrder = new TurnOrder(new List<TurnContext>() { enemy, player });
            TacticianAI ai = await StartTurn(turnOrder, depth, player.Owner);

            BestAction best = ai.ChooseBestMove(turnOrder.ToChainNode(), m_state);

            Assert.That(best.Action, Is.TypeOf<DoNothingAction>());
        }
    }
}