using System;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Room : MonoBehaviour
    {
        private RoomTileData m_roomTileData;
        public Vector2Int WorldPosition => m_roomTileData.WorldPosition;
        public RoomTileData RoomTileData => m_roomTileData;

        public Action<Room> playerEnteredRoom;
        private BoxCollider2D m_boxCollider;

        private void Awake() => m_boxCollider = GetComponent<BoxCollider2D>();

        public void SetRoomTileData(RoomTileData roomTileData)
        {
            m_roomTileData = roomTileData;

            float x = Mathf.Abs(WorldPosition.x - m_roomTileData.LeftMostTile.x) + Mathf.Abs(WorldPosition.x - m_roomTileData.RightMostTile.x);
            float y = Mathf.Abs(WorldPosition.y - m_roomTileData.TopMostTile.y) + Mathf.Abs(WorldPosition.y - m_roomTileData.BottomMostTile.y);
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