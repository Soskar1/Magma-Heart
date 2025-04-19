using System;
using System.Collections.Generic;
using System.Linq;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Room : MonoBehaviour
    {
        private RoomTileData m_roomTileData;
        public Vector2Int WorldPosition => m_roomTileData.WorldPosition;
        public RoomTileData roomTileData => m_roomTileData;

        public Action<Room> playerEnteredRoom;
        private BoxCollider2D m_boxCollider;

        private void Awake() => m_boxCollider = GetComponent<BoxCollider2D>();

        public void Initialize(RoomTileData roomTileData, List<Corridor> corridors)
        {
            m_roomTileData = roomTileData;

            List<Vector2Int> borderPoints = new List<Vector2Int>();
            foreach (Corridor corridor in corridors)
            {
                CorridorEntrance entrance = corridor.Entrance1.RoomTileData == roomTileData ? corridor.Entrance1 : corridor.Entrance2;
                borderPoints.Add(entrance.StartPoint);
            }

            int minX = borderPoints.Min(p => p.x);
            int maxX = borderPoints.Max(p => p.x);
            int minY = borderPoints.Min(p => p.y);
            int maxY = borderPoints.Max(p => p.y);

            float x = Mathf.Abs(WorldPosition.x - minX) + Mathf.Abs(WorldPosition.x - maxX);
            float y = Mathf.Abs(WorldPosition.y - maxY) + Mathf.Abs(WorldPosition.y - minY);

            m_boxCollider.size = new Vector2(x, y);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerBehaviour>() != null)
            {
                m_boxCollider.enabled = false;
                playerEnteredRoom?.Invoke(this);
            }
        }
    }
}