using System.Collections.Generic;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon.Data
{
    [CreateAssetMenu(fileName = "new Room data", menuName = "Magma Heart Data/Rooms/Room Data")]
    public class RoomData : ScriptableObject
    {
        [SerializeField] private RoomType m_roomType;
        [SerializeField] private List<EntityData> m_enemyPool;

        public RoomType RoomType => m_roomType;
        public List<EntityData> EnemyPool => m_enemyPool;
    }
}
