using MagmaHeart.AI.Actions;
using MagmaHeart.Core.Dungeon;

namespace MagmaHeart.Core.CombatSystem
{
    public record MovementActionArgs(RoomTile TileToMove) : ActionArgs;
}
