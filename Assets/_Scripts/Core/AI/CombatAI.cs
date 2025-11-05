using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Collections;
using MagmaHeart.Core.CombatSystem;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Core.AI
{
    public class CombatAI : ICombatTurnSwitchListener
    {
        private readonly TacticianAI m_tactician;

        public CombatAI(TacticianAI tactician)
        {
            m_tactician = tactician;
        }

        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            if (args.CurrentEntity.Model.IsPlayer)
                return;

            List<AIUnit> units = args.CurrentTurnOrder.Select(x => (AIUnit)x.Model).ToList();
            CircularList<AIUnit> turnOrder = new CircularList<AIUnit>();

            foreach (AIUnit unit in units)
                turnOrder.Add(unit);

            Action action = m_tactician.ChooseBestMove(turnOrder);
            action.Execute();

            args.CurrentEntity.CombatController.EndTurn();
        }
    }
}
