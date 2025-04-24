using System;
using System.Collections.Generic;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    [Serializable]
    public struct RoomEnemy
    {
        public EnemyMeleeBehaviour prefab;
        public int count;
    }

    [RequireComponent(typeof(BoxCollider2D))]
    public class Room : MonoBehaviour
    {
        [SerializeField] private float m_colliderSizeModifier;

        private RoomTileData m_roomTileData;
        public RoomTileData roomTileData => m_roomTileData;
        public Vector2Int WorldPosition => m_roomTileData.WorldPosition;
        public List<RoomEnemy> Enemies { get; private set; }

        public Action<Room> playerEnteredRoom;
        private BoxCollider2D m_boxCollider;

        private void Awake() => m_boxCollider = GetComponent<BoxCollider2D>();

        public void Initialize(RoomTileData roomTileData)
        {
            m_roomTileData = roomTileData;
        }

        public void Initialize(RoomTileData roomTileData, List<Corridor> corridors, List<RoomEnemy> enemies)
        {
            m_roomTileData = roomTileData;
            Enemies = enemies;

            float minDistance = float.MaxValue;
            foreach (Corridor corridor in corridors)
            {
                CorridorEntrance entrance = corridor.Entrance1.RoomTileData == roomTileData ? corridor.Entrance1 : corridor.Entrance2;
                float distance = Vector2Int.Distance(roomTileData.WorldPosition, entrance.StartPoint);
                if (distance < minDistance)
                    minDistance = distance;
            }

            m_boxCollider.size = new Vector2(minDistance, minDistance) * m_colliderSizeModifier;
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