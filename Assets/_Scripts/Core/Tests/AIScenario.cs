using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Tests
{
    internal record AIScenario(Board Board, CircularList<AIUnitModel> TurnOrder)
    {
        public async Task<BestPlan> RunAI(int depth, ActionDatabase actionDatabase)
        {
            AggressiveStrategy strategy = new AggressiveStrategy();
            MagmaHeartTurnContext turnContext = new MagmaHeartTurnContext();
            AIEngine ai = new AIEngine(strategy, actionDatabase, depth, turnContext);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            IEnumerable<IBoardCommand> commands = turnContext.StartTurn(TurnOrder.Head);
            CommandRunner runner = new CommandRunner();
            runner.Apply(Board, commands);

            CircularList<int> modelTurns = new CircularList<int>();
            foreach (AIUnitModel entity in TurnOrder)
                modelTurns.Add(entity.Id);

            return ai.ChooseBestMove(modelTurns, Board);
        }
    }
}
