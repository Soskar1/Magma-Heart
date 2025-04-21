using MagmaHeart.Core.Dungeon;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Tests
{
    public class LocationGeneratorTest : MonoBehaviour
    {
        [SerializeField] private LocationGenerator m_locationGenerator;
        [SerializeField] private LocationRenderer m_renderer;

        [Header("GFX")]
        [SerializeField] private Tilemap m_floor;
        [SerializeField] private Tilemap m_walls;
        [SerializeField] private Tilemap m_corridorEntrances;
        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private TileBase m_wallTile;

        public async void Generate()
        {
            m_renderer.Clear();
            Location location = await m_locationGenerator.GenerateLocation(Vector2Int.zero);
            m_renderer.AddTilesToDraw(location.FloorTiles, m_floor, m_floorTile);
            m_renderer.AddTilesToDraw(location.WallTiles, m_walls, m_wallTile);
            m_renderer.AddTilesToDraw(location.CorridorEntranceTiles, m_corridorEntrances, m_wallTile);
            m_renderer.DrawTiles();
        }
    }
}