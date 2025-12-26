using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using System.Threading.Tasks;
using System.Threading;

namespace MagmaHeart.Core.Tests
{
    internal record AIScenario(CombatBoardState State, TurnOrder TurnOrder)
    {
        public async Task<BestPlan> RunAI(int depth, ActionDatabase actionDatabase)
        {
            AggressiveStrategy strategy = new AggressiveStrategy();
            CombatAI ai = new CombatAI(strategy, actionDatabase, depth);

            OnBattleStartedEventArgs args = new OnBattleStartedEventArgs(TurnOrder, State);
            ai.HandleOnBattleStarted(this, args);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await TurnOrder.Current.StartTurnAsync(State, cancellationTokenSource.Token);

            return ai.GetBestAction();
        }
    }
}
