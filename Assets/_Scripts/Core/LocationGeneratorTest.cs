using MagmaHeart.Core.Dungeon;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core
{
    public class LocationGeneratorTest : MonoBehaviour
    {
        [SerializeField] private LocationGenerator m_locationGenerator;
        [SerializeField] private LocationRenderer m_renderer;

        public async void Generate()
        {
            m_renderer.Clear();
            Location location = await m_locationGenerator.GenerateLocation(Vector2Int.zero);

            HashSet<DungeonTile> tiles = new HashSet<DungeonTile>();
            tiles.UnionWith(location.CorridorTiles);

            foreach (RoomData roomData in location.Rooms)
                tiles.UnionWith(roomData.GetTiles());

            StartCoroutine(m_renderer.DrawTiles(tiles));
        }
    }
}