using MagmaHeart.AI.Execution;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.Collections;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class EnemyTurnController : ITurnController
    {
        private readonly AIEngine m_aiEngine;
        private readonly ActionExecutor m_actionRunner;
        private CancellationTokenSource m_cancellationTokenSource;

        public EnemyTurnController(AIEngine engine, ActionExecutor actionRunner)
        {
            m_aiEngine = engine;
            m_actionRunner = actionRunner;
        }

        public async Task StartTurn(Room room, TurnOrder turnOrder)
        {
            CircularList<int> modelTurns = new CircularList<int>();
            foreach (Entity entity in turnOrder)
                modelTurns.Add(entity.Model.Id);
            
            BestPlan bestPlan = m_aiEngine.ChooseBestMove(modelTurns, room);
            m_cancellationTokenSource = new CancellationTokenSource();

            if (bestPlan != null)
            {
                foreach (ExecutedTask task in bestPlan.ExecutedTasks)
                {
                    IEnumerable<IBoardCommand> commands = task.Action.Execute(task.Args, room);
                    await m_actionRunner.ApplyAsync(room, commands, m_cancellationTokenSource.Token);
                }
            }

            EndTurn();
        }

        public void EndBattle()
        {
            if (m_cancellationTokenSource != null)
                m_cancellationTokenSource.Cancel();
        }

        public void EndTurn()
        {
            if (!m_cancellationTokenSource.IsCancellationRequested)
                m_cancellationTokenSource.Cancel();
        }
    }
}
