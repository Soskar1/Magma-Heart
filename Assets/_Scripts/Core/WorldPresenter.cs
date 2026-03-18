using MagmaHeart.Core.Dungeon;
using MagmaHeart.DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core
{
    public class WorldPresenter : MonoBehaviour
    {
        [SerializeField] private Tilemap m_tilemap;
        [SerializeField] private Tilemap m_decorations;
        [SerializeField] private int m_tilesPerFrame = 256;
        [SerializeField] private TileBase m_floor;
        [SerializeField] private TileBase m_wall;
        [SerializeField] private TileBase m_door;

        [SerializeField] private float m_decorationChance = 0.1f;

        [SerializeField] private List<TileBase> m_decorationTiles;

        private GameWorld m_gameWorld;

        private TaskCompletionSource<bool> m_renderedRoom;
        public Task<bool> OnRoomRendered => m_renderedRoom.Task;

        public void Initialize(GameWorld gameWorld)
        {
            m_gameWorld = gameWorld;
            gameWorld.OnRoomGenerated += HandleOnRoomGenerated;
        }

        public void OnDisable() => m_gameWorld.OnRoomGenerated -= HandleOnRoomGenerated;

        public void HandleOnRoomGenerated(object obj, OnRoomGeneratedEventArgs args)
        {
            Clear();

            m_renderedRoom = new TaskCompletionSource<bool>();
            StartCoroutine(DrawTiles(args.Room.RoomModel));
        }

        private IEnumerator DrawTiles(RoomModel roomModel)
        {
            int renderedTiles = 0;
            foreach (DungeonTile tile in roomModel)
            {
                Vector3Int tilePosition = m_tilemap.WorldToCell((Vector3Int)tile.Position);

                if (tile == roomModel.EntranceDoor || tile == roomModel.ExitDoor)
                {
                    m_tilemap.SetTile(tilePosition, m_door);
                }
                else if (tile.Type == TileType.Floor)
                {
                    m_tilemap.SetTile(tilePosition, m_floor);

                    if (Random.value < m_decorationChance)
                    {
                        TileBase decorationTile = m_decorationTiles[Random.Range(0, m_decorationTiles.Count)];
                        m_decorations.SetTile(tilePosition, decorationTile);
                    }
                }
                else
                {
                    m_tilemap.SetTile(tilePosition, m_wall);
                }

                ++renderedTiles;

                if (renderedTiles % m_tilesPerFrame == 0)
                    yield return new WaitForEndOfFrame();
            }

            m_renderedRoom.SetResult(true);
        }

        public void Clear()
        {
            m_decorations.ClearAllTiles();
            m_tilemap.ClearAllTiles();
        }
    }
}