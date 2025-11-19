using MagmaHeart.Core.BoardStateSystem;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public abstract class ActionSelector
    {
        public ActionSelector Next { get; set; }

        public ActionSelectionResult GetAction(CombatBoardState combatBoardState, RoomTile selectedTile)
        {
            ActionSelectionResult result = null;
            ActionSelector selector = this;

            while (selector != null)
            {
                result = selector.TrySelectAction(combatBoardState, selectedTile);

                if (result != null)
                    break;

                selector = selector.Next;
            }

            return result;
        }

        protected abstract ActionSelectionResult TrySelectAction(CombatBoardState combatBoardState, RoomTile selectedTile);
    }
}
