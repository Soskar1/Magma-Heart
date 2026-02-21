using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Input.Mouse
{
    public class TileMouseHoverStrategy : MouseHoverStrategy
    {
        private readonly GameWorld gameWorld;

        public TileMouseHoverStrategy(GameWorld gameWorld)
        {
            this.gameWorld = gameWorld;
        }

        protected override HoverResult TryHover(Vector2 worldPosition)
        {
            Vector3Int tilePosition = gameWorld.WorldToTilePosition(worldPosition);
            RoomTile hoveredTile = gameWorld.GetTile(tilePosition);
            gameWorld.TryGetEntityAtPosition(hoveredTile.Position, out Entity entity);

            return new CombatHoverResult(entity, hoveredTile);
        }
    }
}
