using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.Execution;
using MagmaHeart.Collections;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using System.Threading;

namespace MagmaHeart.Core.Tests
{
    internal record AIScenario(Board Board, CircularList<AIUnitModel> TurnOrder)
    {
        public BestPlan RunAI(int depth)
        {
            AggressiveStrategy strategy = new AggressiveStrategy();
            IStartOfTurnCommandFactory factory = new StartOfTurnCommandFactory();
            AIEngine ai = new AIEngine(strategy, depth, factory);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            IEnumerable<IBoardCommand> commands = factory.BuildStartOfTurnCommands(Board, TurnOrder.Head);
            //CommandRunner runner = new CommandRunner();
            //runner.Apply(Board, commands);
            throw new System.NotImplementedException("AIScenario.RunAI: Apply the start of turn commands here");

            CircularList<int> modelTurns = new CircularList<int>();
            foreach (AIUnitModel entity in TurnOrder)
                modelTurns.Add(entity.Id);

            return ai.ChooseBestMove(modelTurns, Board);
        }
    }
}
