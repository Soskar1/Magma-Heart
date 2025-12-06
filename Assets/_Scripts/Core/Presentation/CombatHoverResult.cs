using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Presentation
{
    public record CombatHoverResult(Entity Entity, RoomTile RoomTile) : HoverResult(Entity);
}
