using MagmaHeart.Core.BoardStateSystem;
using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnBattleStartedEventArgs : EventArgs
    {
        public TurnOrder TurnOrder { get; init; }
        public CombatBoardState CombatBoardState { get; init; }

        public OnBattleStartedEventArgs(TurnOrder turnOrder, CombatBoardState combatBoardState)
        {
            TurnOrder = turnOrder;
            CombatBoardState = combatBoardState;
        }
    }
}
