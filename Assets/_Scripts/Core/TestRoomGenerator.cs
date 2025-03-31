using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core
{
    public class TestRoomGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int m_roomSize;

        public RoomData CreateRoom()
        {
            BoxedRoomGenerator boxedRoomGenerator = new BoxedRoomGenerator(m_roomSize.x, m_roomSize.y);

            BoundsInt roomSpace = new BoundsInt(new Vector3Int((int)transform.position.x - m_roomSize.x / 2, (int)transform.position.y - m_roomSize.y / 2, 0),
                new Vector3Int(m_roomSize.x, m_roomSize.y, 0));

            RoomData roomData = new RoomData(roomSpace);
            boxedRoomGenerator.GenerateRoom(roomData);

            return roomData;
        }        
    }    
}
