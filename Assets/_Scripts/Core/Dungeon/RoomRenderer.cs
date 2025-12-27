using System.Collections;
using System.Threading.Tasks;
using MagmaHeart.DungeonGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomRenderer : MonoBehaviour
    {
        [SerializeField] private int m_tilesPerFrame = 256;
        private Tilemap m_tilemap;

        private TaskCompletionSource<bool> m_renderedAllTiles;

        public void Initialize(Tilemap tilemap) => m_tilemap = tilemap;

        public async Task DrawRoom(RoomModel roomModel, TileBase floor, TileBase wall)
        {
            m_renderedAllTiles = new TaskCompletionSource<bool>();
            StartCoroutine(DrawTiles(roomModel, floor, wall));
            await m_renderedAllTiles.Task;
        }

        private IEnumerator DrawTiles(RoomModel roomModel, TileBase floor, TileBase wall)
        {
            int renderedTiles = 0;
            foreach (DungeonTile tile in roomModel)
            {
                Vector3Int tilePosition = m_tilemap.WorldToCell((Vector3Int)tile.Position);

                if (tile.Type == TileType.Floor)
                    m_tilemap.SetTile(tilePosition, floor);
                else
                    m_tilemap.SetTile(tilePosition, wall);

                ++renderedTiles;

                if (renderedTiles % m_tilesPerFrame == 0)
                    yield return new WaitForEndOfFrame();
            }

            m_renderedAllTiles.SetResult(true);
        }

        public void Clear() => m_tilemap.ClearAllTiles();
    }
}