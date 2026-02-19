using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities
{
    public interface ITurnController
    {
        public Task StartTurn(Room room, TurnOrder turnOrder);
        public void EndTurn();
        public void EndBattle();
    }
}
