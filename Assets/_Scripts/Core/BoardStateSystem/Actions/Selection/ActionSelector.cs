using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public abstract class ActionSelector
    {
        public ActionSelector Next { get; set; }

        public ActionSelectionResult GetAction(Room room, EntityModel executor, RoomTile selectedTile)
        {
            ActionSelectionResult result = null;
            ActionSelector selector = this;

            while (selector != null)
            {
                result = selector.TrySelectAction(room, executor, selectedTile);

                if (result != null)
                    break;

                selector = selector.Next;
            }

            return result;
        }

        protected abstract ActionSelectionResult TrySelectAction(Room room, EntityModel executor, RoomTile selectedTile);
    }
}
