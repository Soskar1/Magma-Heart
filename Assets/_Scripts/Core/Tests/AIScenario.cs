using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Collections;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Tests
{
    internal record AIScenario(CombatBoardState State, CircularList<AIUnitModel> TurnOrder)
    {
        public async Task<BestPlan> RunAI(int depth, ActionDatabase actionDatabase)
        {
            AggressiveStrategy strategy = new AggressiveStrategy();
            MagmaHeartTurnContext turnContext = new MagmaHeartTurnContext();
            AIEngine ai = new AIEngine(strategy, actionDatabase, depth, turnContext);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await turnContext.StartTurnAsync(State, TurnOrder.Head, cancellationTokenSource.Token);

            CircularList<int> modelTurns = new CircularList<int>();
            foreach (AIUnitModel entity in TurnOrder)
                modelTurns.Add(entity.Id);

            return ai.ChooseBestMove(modelTurns, State);
        }
    }
}
