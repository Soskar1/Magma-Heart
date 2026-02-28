using MagmaHeart.Abilities;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Collections;
using MagmaHeart.Core.Abilities.Presentation.Execution;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class EnemyTurnController
    {
        private readonly AIEngine m_aiEngine;
        private readonly AbilityExecutionRunner m_abilityExecutionRunner;
        private CancellationTokenSource m_cancellationTokenSource;

        public EnemyTurnController(AIEngine engine, AbilityExecutionRunner abilityExecutionRunner)
        {
            m_aiEngine = engine;
            m_abilityExecutionRunner = abilityExecutionRunner;
        }

        public async Task StartTurn(Room room, TurnOrder turnOrder)
        {
            CircularList<int> modelTurns = new CircularList<int>();
            foreach (Entity entity in turnOrder)
                modelTurns.Add(entity.Model.Id);
            
            //BestPlan bestPlan = m_aiEngine.ChooseBestMove(modelTurns, room);
            m_cancellationTokenSource = new CancellationTokenSource();

            //if (bestPlan != null)
            //{
            //    foreach (AbilityPlan ability in bestPlan.ExecutedTasks)
            //    {
            //        if (m_cancellationTokenSource.IsCancellationRequested)
            //            break;

            //        await m_abilityExecutionRunner.Run(ability, turnOrder.Current.Model.Id, m_cancellationTokenSource.Token);
            //    }
            //}

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
