using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Input.Mouse
{
    public record CombatHoverResult(Entity Entity, RoomTile RoomTile) : HoverResult
    {
        public override void Accept(IHoverResultVisitor visitor) => visitor.Visit(this);
    }
}
