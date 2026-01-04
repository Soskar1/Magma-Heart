using UnityEngine;

namespace MagmaHeart.Core.Dungeon.Data
{
    [CreateAssetMenu(fileName = "new Location data", menuName = "Magma Heart Data/Location Data")]
    public class LocationData : ScriptableObject
    {
        [SerializeField] private string m_roomGenerationConfigFileName;
        [SerializeField] private int m_roomsInLocation = 8;
        [SerializeField] private RoomData m_monsterRoom;
        [SerializeField] private RoomData m_bossRoom;

        public string RoomGenerationConfigFileName => m_roomGenerationConfigFileName;
        public int RoomsInLocation => m_roomsInLocation;
        public RoomData BossRoom => m_bossRoom;
        public RoomData MonsterRoom => m_monsterRoom;
    }
}
