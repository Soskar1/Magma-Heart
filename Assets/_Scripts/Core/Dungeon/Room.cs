using System;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Room : MonoBehaviour
    {
        private RoomTileData m_roomTileData;
        public RoomTileData roomTileData => m_roomTileData;
        public CombatData CombatData { get; private set; }

        public Action<Room> playerEnteredRoom;
        private BoxCollider2D m_boxCollider;

        private void Awake() => m_boxCollider = GetComponent<BoxCollider2D>();

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