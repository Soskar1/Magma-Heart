using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon.Data
{
    [CreateAssetMenu(fileName = "new Location data", menuName = "Magma Heart Data/Location Data")]
    public class LocationData : ScriptableObject
    {
        [SerializeField] private string m_roomGenerationConfigFileName;
        [SerializeField] private List<RoomData> m_rooms;

        public string RoomGenerationConfigFileName => m_roomGenerationConfigFileName;
        public List<RoomData> Rooms => m_rooms;
    }
}
