using System.Collections.Generic;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;

namespace MagmaHeart.Core.StateMachines
{
    public class CombatState : IState
    {
        private readonly List<ICombatStateListener> m_combatStateListeners;
        private readonly Battle m_battle;

        public CombatState(Battle battle, List<ICombatStateListener> combatStateListeners)
        {
            m_battle = battle;
            m_combatStateListeners = combatStateListeners;
        }

        public async void Enter(params object[] args)
        {
            Room room = args[0] as Room;

            foreach (ICombatStateListener listener in m_combatStateListeners)
                listener.EnterCombatState();

            await m_battle.Start(room);
        }

        public void Exit()
        {
            foreach (ICombatStateListener listener in m_combatStateListeners)
                listener.ExitCombatState();
        }
    }
}