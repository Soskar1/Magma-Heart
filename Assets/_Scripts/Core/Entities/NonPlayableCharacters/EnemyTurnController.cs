using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.Collections;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.CombatSystem;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class EnemyTurnController : ITurnController
    {
        private readonly AIEngine m_aiEngine;
        private CancellationTokenSource m_cancellationTokenSource;

        public EnemyTurnController(AIEngine engine)
        {
            m_aiEngine = engine;
        }

        public async Task StartTurn(CombatBoardState boardState, TurnOrder turnOrder)
        {
            CircularList<AIUnitModel> modelTurns = new CircularList<AIUnitModel>();
            foreach (Entity entity in turnOrder)
                modelTurns.Add(entity.Model);
            
            BestPlan bestPlan = m_aiEngine.ChooseBestMove(modelTurns, boardState);
            m_cancellationTokenSource = new CancellationTokenSource();

            if (bestPlan != null)
            {
                foreach (ExecutedTask task in bestPlan.ExecutedTasks)
                {
                    if (m_cancellationTokenSource.Token.IsCancellationRequested)
                        break;

                    await task.Action.ExecuteAsync(task.Args, boardState, m_cancellationTokenSource.Token);
                }
            }

            EndTurn();
        }

        public void EndBattle()
        {
            m_cancellationTokenSource.Cancel();
        }

        public void EndTurn()
        {
            if (!m_cancellationTokenSource.IsCancellationRequested)
                m_cancellationTokenSource.Cancel();
        }
    }
}
