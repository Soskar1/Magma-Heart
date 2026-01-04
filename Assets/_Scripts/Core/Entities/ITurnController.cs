using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.CombatSystem;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities
{
    public interface ITurnController
    {
        public Task StartTurn(CombatBoardState state, TurnOrder turnOrder);
        public void EndTurn();
        public void EndBattle();
    }
}
