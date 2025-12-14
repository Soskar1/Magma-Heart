using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public abstract class ActionSelector
    {
        public ActionSelector Next { get; set; }

        public ActionSelectionResult GetAction(CombatBoardState combatBoardState, EntityModel executor, RoomTile selectedTile)
        {
            ActionSelectionResult result = null;
            ActionSelector selector = this;

            while (selector != null)
            {
                result = selector.TrySelectAction(combatBoardState, executor, selectedTile);

                if (result != null)
                    break;

                selector = selector.Next;
            }

            return result;
        }

        protected abstract ActionSelectionResult TrySelectAction(CombatBoardState combatBoardState, EntityModel executor, RoomTile selectedTile);
    }
}
