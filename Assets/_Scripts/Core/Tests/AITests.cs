using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal class AITests : CoreTests
    {
        private ActionDatabase m_actionDatabase;

        private async Task<CombatAI> StartTurn(TurnOrder turnOrder, int lookAhead, AIUnitModel player)
        {
            AggressiveStrategy strategy = new AggressiveStrategy(player);
            CombatAI ai = new CombatAI(strategy, m_actionDatabase, lookAhead);
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            OnBattleStartedEventArgs args = new OnBattleStartedEventArgs(turnOrder, State);
            ai.HandleOnBattleStarted(this, args);

            await turnOrder.Current.StartTurnAsync(State, tokenSource.Token);

            return ai;
        }

        private void CreateWall(Vector2 position) => State.Room.ChangeNodeType(position, BoardNodeType.Obstacle);

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

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Assembly assembly = FindAssembly("MagmaHeart.Core");
            m_actionDatabase = new ActionDatabase(assembly);
        }

        Assembly FindAssembly(string assemblyName)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName);
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
            TurnContext<EntityModel> enemy = AddEntity(enemyPosition, false);
            TurnOrder turnOrder = new TurnOrder(new List<TurnContext>() { enemy, player });
            CombatAI ai = await StartTurn(turnOrder, depth, player.Model);

            BestAction best = ai.GetBestAction();

            Assert.That(best.Action, Is.TypeOf<MovementAction>());
            
            //MovementActionPayload payload = new MovementActionPayload(MovementAction.MOVEMENT_DISTANCE_IN_TILES_FOR_ONE_ENERGY);
            //Assert.That(best.Args, Is.EqualTo(new MovementActionArgs(enemy.TypedModel, enemyPosition.ToVector2(), new Vector2(3, 3), payload)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task AttackAction_AggressiveStrategy_AttacksPlayer(int depth)
        {
            TurnContext<EntityModel> player = AddEntity(new Vector3Int(2, 3), true);
            TurnContext<EntityModel> enemy = AddEntity(new Vector3Int(3, 3), false);
            TurnOrder turnOrder = new TurnOrder(new List<TurnContext>() { enemy, player });
            CombatAI ai = await StartTurn(turnOrder, depth, player.Model);

            BestAction best = ai.GetBestAction();

            Assert.That(best.Action, Is.TypeOf<AttackAction>());
            //AttackActionPayload payload = new AttackActionPayload(AttackAction.ENERGY_COST, AttackAction.ATTACK_DISTANCE, AttackAction.ATTACK_DAMAGE);
            //Assert.That(best.Args, Is.EqualTo(new AttackActionArgs(enemy.TypedModel, payload, player.TypedModel)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task AttackAction_EnemyLowHPAggressiveStrategy_AttacksPlayer(int depth)
        {
            TurnContext<EntityModel> player = AddEntity(new Vector3Int(2, 3), true);
            TurnContext<EntityModel> enemy = AddEntity(new Vector3Int(3, 3), false, 1);
            TurnOrder turnOrder = new TurnOrder(new List<TurnContext>() { enemy, player });
            CombatAI ai = await StartTurn(turnOrder, depth, player.Model);

            BestAction best = ai.GetBestAction();

            Assert.That(best.Action, Is.TypeOf<AttackAction>());
            //AttackActionPayload payload = new AttackActionPayload(AttackAction.ENERGY_COST, AttackAction.ATTACK_DISTANCE, AttackAction.ATTACK_DAMAGE);
            //Assert.That(best.Args, Is.EqualTo(new AttackActionArgs(enemy.TypedModel, payload, player.TypedModel)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task DoNothingAction_PlayerSurroundedByWalls_EnemyDoesNothing(int depth)
        {
            TurnContext player = AddEntity(new Vector3Int(2, 3), true);
            SurroundEntityWithWalls((EntityModel)player.Model);
            TurnContext enemy = AddEntity(new Vector3Int(4, 3), false, 1);
            TurnOrder turnOrder = new TurnOrder(new List<TurnContext>() { enemy, player });
            CombatAI ai = await StartTurn(turnOrder, depth, player.Model);

            BestAction best = ai.GetBestAction();

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
            SurroundEntityWithWalls((EntityModel)enemy.Model);
            TurnOrder turnOrder = new TurnOrder(new List<TurnContext>() { enemy, player });
            CombatAI ai = await StartTurn(turnOrder, depth, player.Model);

            BestAction best = ai.GetBestAction();

            Assert.That(best.Action, Is.TypeOf<DoNothingAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task MovementAction_OnePlayerTwoEnemiesWithLowHealth_EnemyMovesTowardsPlayer(int depth)
        {
            TurnContext player = AddEntity(new Vector3Int(3, 3), true, 4);
            TurnContext enemy1 = AddEntity(new Vector3Int(1, 1), false, 1);
            TurnContext enemy2 = AddEntity(new Vector3Int(3, 2), false, 2);
            TurnOrder turnOrder = new TurnOrder(new List<TurnContext>() { enemy1, player, enemy2 });
            CombatAI ai = await StartTurn(turnOrder, depth, player.Model);

            BestAction best = ai.GetBestAction();

            Assert.That(best.Action, Is.TypeOf<MovementAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task MovementAction_OnePlayerTwoEnemies_EnemyMovesTowardsPlayer(int depth)
        {
            TurnContext player = AddEntity(new Vector3Int(2, 2), true, 5);
            TurnContext enemy1 = AddEntity(new Vector3Int(2, 1), false, 5);
            TurnContext enemy2 = AddEntity(new Vector3Int(0, 1), false, 2);
            TurnOrder turnOrder = new TurnOrder(new List<TurnContext>() { enemy2, player, enemy1});
            CombatAI ai = await StartTurn(turnOrder, depth, player.Model);

            BestAction best = ai.GetBestAction();

            Assert.That(best.Action, Is.TypeOf<MovementAction>());
        }
    }
}