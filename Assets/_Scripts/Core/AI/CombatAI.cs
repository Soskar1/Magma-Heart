using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Collections;
using MagmaHeart.Core.Dungeon;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class CombatAI
    {
        private readonly TacticianAI m_tactician;

        public CombatAI(Strategy strategy) {
            m_tactician = new TacticianAI(strategy);
        }

        public BestAction GetBestAction(CircularList<Entity> turnOrder, Room room)
        {
            List<AIUnit> units = turnOrder.Select(x => (AIUnit)x.Model).ToList();
            CircularList<AIUnit> turnOrderToProcess = new CircularList<AIUnit>();

            foreach (AIUnit unit in units)
                turnOrderToProcess.Add(unit);

            return m_tactician.ChooseBestMove(turnOrderToProcess, room);
        }
    }
}
