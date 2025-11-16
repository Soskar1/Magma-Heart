using MagmaHeart.Core.Dungeon;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public abstract class ActionSelector
    {
        public ActionSelector Next { get; set; }

        public ActionSelectionResult GetAction(Room room, RoomTile roomTile)
        {
            ActionSelectionResult result = null;
            ActionSelector selector = this;

            while (selector != null)
            {
                result = selector.TrySelectAction(room, roomTile);

                if (result != null)
                    break;

                selector = selector.Next;
            }

            return result;
        }

        protected abstract ActionSelectionResult TrySelectAction(Room room, RoomTile roomTile);
    }
}
