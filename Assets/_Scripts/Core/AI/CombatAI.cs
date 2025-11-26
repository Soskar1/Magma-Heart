using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.CombatSystem;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class CombatAI
    {
        private readonly TacticianAI m_tactician;
        private TurnOrder m_currentTurnOrder;
        private CombatBoardState m_currentBoardState;

        public CombatAI(Strategy strategy) {
            m_tactician = new TacticianAI(strategy);
        }

        public BestAction GetBestAction()
        {
            ChainNode<TurnContext> chain = m_currentTurnOrder.ToChainNode();
            BestAction bestAction = m_tactician.ChooseBestMove(chain, m_currentBoardState);

            if (bestAction == null)
            {
                bestAction = new BestAction(
                    new DoNothingAction((EntityModel)m_currentTurnOrder.Current.Owner),
                    ActionArgs.Empty);
            }

            return bestAction;
        }

        public void HandleOnBattleStarted(object obj, OnBattleStartedEventArgs args)
        {
            m_currentTurnOrder = args.TurnOrder;
            m_currentBoardState = args.CombatBoardState;
        }
    }
}
