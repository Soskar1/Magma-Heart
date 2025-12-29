using MagmaHeart.DungeonGeneration;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomRenderer : MonoBehaviour
    {
        [SerializeField] private Tilemap m_tilemap;
        [SerializeField] private int m_tilesPerFrame = 256;
        [SerializeField] private TileBase m_floor;
        [SerializeField] private TileBase m_wall;
        [SerializeField] private TileBase m_door;

        private DungeonController m_controller;

        public void Initialize(DungeonController dungeonController)
        {
            m_controller = dungeonController;
            dungeonController.OnRoomGenerated += HandleOnRoomGenerated;
        }

        public void OnDisable() => m_controller.OnRoomGenerated -= HandleOnRoomGenerated;

        public void HandleOnRoomGenerated(object obj, OnRoomGeneratedEventArgs args)
        {
            Clear();

            StartCoroutine(DrawTiles(args.Room.RoomModel));
        }

        private IEnumerator DrawTiles(RoomModel roomModel)
        {
            int renderedTiles = 0;
            foreach (DungeonTile tile in roomModel)
            {
                Vector3Int tilePosition = m_tilemap.WorldToCell((Vector3Int)tile.Position);

                if (tile == roomModel.EntranceDoor || tile == roomModel.ExitDoor)
                    m_tilemap.SetTile(tilePosition, m_door);
                else if (tile.Type == TileType.Floor)
                    m_tilemap.SetTile(tilePosition, m_floor);
                else
                    m_tilemap.SetTile(tilePosition, m_wall);

                ++renderedTiles;

                if (renderedTiles % m_tilesPerFrame == 0)
                    yield return new WaitForEndOfFrame();
            }
        }

        public void Clear() => m_tilemap.ClearAllTiles();
    }
}