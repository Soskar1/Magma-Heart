using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    public class TestLocationGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int m_roomSize;

        public Location GenerateLocation()
        {
            BoxedRoomGenerator boxedRoomGenerator = new BoxedRoomGenerator(m_roomSize.x, m_roomSize.y);

            BoundsInt roomSpace = new BoundsInt(new Vector3Int((int)transform.position.x - m_roomSize.x / 2, (int)transform.position.y - m_roomSize.y / 2, 0),
                new Vector3Int(m_roomSize.x, m_roomSize.y, 0));

            RoomData roomData = new RoomData(roomSpace);
            boxedRoomGenerator.GenerateRoom(roomData);

            LocationWallGenerator wallGenerator = new LocationWallGenerator();
            HashSet<DungeonTile> walls = wallGenerator.GenerateWalls(roomData.GetTilePositions());

            return new Location(new List<RoomData>() { roomData }, new HashSet<DungeonTile>(), walls);
        }        
    }    
}
