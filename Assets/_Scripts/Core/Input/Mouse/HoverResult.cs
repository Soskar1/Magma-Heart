using MagmaHeart.Core.Entities;
using MagmaHeart.DungeonGeneration;
using UnityEngine;

namespace MagmaHeart.Core.Input.Mouse
{
    public record HoverResult(HoverResultType Type, Entity Entity, DungeonTile Tile, GameObject UI)
    {
        public HoverResultType Type { get; private set; } = Type;
        public Entity Entity { get; private set; } = Entity;
        public DungeonTile Tile { get; private set; } = Tile;
        public GameObject UI { get; private set; } = UI;

        public static HoverResult None => new HoverResult(HoverResultType.None, default, default, default);
        public static HoverResult EntityHover(Entity entity) => new HoverResult(HoverResultType.Entity, entity, default, default);
        public static HoverResult TileHover(DungeonTile tile) => new HoverResult(HoverResultType.Tile, default, tile, default);

        public HoverResult AppendUIInfo(GameObject ui)
        {
            Type |= HoverResultType.UI;
            UI = ui;
            return this;
        }

        public HoverResult AppendTileInfo(DungeonTile tile)
        {
            Type |= HoverResultType.Tile;
            Tile = tile;
            return this;
        }

        public HoverResult AppendEntityInfo(Entity entity)
        {
            Type |= HoverResultType.Entity;
            Entity = entity;
            return this;
        }
    }
}
