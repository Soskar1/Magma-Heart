using MagmaHeart.Abilities;
using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.Collections;
using System.Collections.Generic;

namespace MagmaHeart.Core.Tests
{
    internal record AIScenario(Board Board, CircularList<AIUnitModel> TurnOrder)
    {
        public IEnumerable<AbilityPlan> RunAI(int depth)
        {
            //AggressiveStrategy strategy = new AggressiveStrategy();
            //AIEngine ai = new AIEngine(strategy, depth);

            //using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            //// IEnumerable<IBoardCommand> commands = factory.BuildStartOfTurnCommands(Board, TurnOrder.Head);
            //// CommandRunner runner = new CommandRunner();
            //// runner.Apply(Board, commands);
            //throw new System.NotImplementedException("AIScenario.RunAI: Apply the start of turn commands here");

            //CircularList<int> modelTurns = new CircularList<int>();
            //foreach (AIUnitModel entity in TurnOrder)
            //    modelTurns.Add(entity.Id);

            //return ai.ChooseBestMove(modelTurns, Board);

            return null;
        }
    }
}
