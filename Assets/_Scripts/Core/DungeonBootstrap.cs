using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core
{
    public class DungeonBootstrap : MonoBehaviour
    {
        [SerializeField] private Player m_player;
        [SerializeField] private LocationGenerator m_locationGenerator;
        [SerializeField] private LocationRenderer m_renderer;

        private Location m_location;

        private void Awake() => m_renderer.RenderedAllTiles += SpawnPlayer;

        private void Start() => BootScene();

        private async void BootScene()
        {
            m_location = await m_locationGenerator.GenerateLocation(Vector2Int.zero);

            HashSet<Vector2Int> tiles = new HashSet<Vector2Int>();
            tiles.UnionWith(m_location.CorridorTiles);

            foreach (RoomData roomData in m_location.Rooms)
                tiles.UnionWith(roomData.GetTilesCopy());

            StartCoroutine(m_renderer.DrawTiles(tiles));
        }

        private void SpawnPlayer()
        {
            RoomData roomData = m_location.Rooms[Random.Range(0, m_location.Rooms.Count)];
            Instantiate(m_player, (Vector2)roomData.WorldPosition, Quaternion.identity);
            m_renderer.RenderedAllTiles -= SpawnPlayer;
        }
    }
}