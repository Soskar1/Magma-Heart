using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationGenerator : MonoBehaviour
    {
        private LocationRenderer m_renderer;

        [SerializeField] private Tilemap m_tilemap;
        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private TileBase m_wallTile;
        [SerializeField] private int m_xBorderSize;
        [SerializeField] private int m_yBorderSize;

        [Header("RandomWalkRoomGenerator")]
        [SerializeField] private int m_randomWalkIterations;
        
        [Header("BoxedRoomGenerator")]
        [SerializeField] private int m_xSize;
        [SerializeField] private int m_ySize;

        [Header("DiffusionLimitedAggregatoinRoomGenerator")]
        [SerializeField] private int m_tilesToPlace;

        [Header("TilePropagation")]
        [SerializeField] private int m_propagationLength;

        [Header("BinarySpacePartitioning")]
        [SerializeField] private int m_xMinSize;
        [SerializeField] private int m_yMinSize;
        [SerializeField] private int m_maxPartitions;

        private void Awake()
        {
            m_renderer = new LocationRenderer(m_tilemap, m_floorTile, m_wallTile);
        }

        public void GenerateLocation() => GenerateLocation(Vector2Int.zero);

        public void GenerateLocation(in Vector2Int position)
        {
            BinarySpacePartitioning spacePartitioning = new BinarySpacePartitioning(m_xMinSize, m_yMinSize, m_maxPartitions);
            BoundsInt locationSpace = new BoundsInt(new Vector3Int(position.x - m_xBorderSize / 2, position.y - m_yBorderSize / 2, 0), new Vector3Int(m_xBorderSize, m_yBorderSize, 0));
            List<BoundsInt> spaces = spacePartitioning.PerformBinarySpacePartitioning(locationSpace);

            HashSet<Vector2Int> generatedTiles = new HashSet<Vector2Int>();

            foreach (BoundsInt space in spaces)
            {
                Vector2Int roomPosition = new Vector2Int((int)space.center.x, (int)space.center.y);
                RoomData roomData = new RoomData(roomPosition, space.size.x - 5, space.size.y - 5);

                IRoomGenerator generator1 = new BoxedRoomGenerator(roomData, m_xSize, m_ySize); 
                IRoomGenerator generator2 = new RandomWalkRoomGenerator(roomData, m_randomWalkIterations);
                IRoomGenerator generator3 = new DiffusionLimitedAggregatoinRoomGenerator(roomData, m_tilesToPlace);
                IRoomGenerator generator4 = new RandomWalkRoomGenerator(roomData, m_randomWalkIterations);
                IRoomGenerator generator5 = new DiffusionLimitedAggregatoinRoomGenerator(roomData, m_tilesToPlace);
                IRoomModifier modifier1 = new TilePropagation(roomData, m_propagationLength);
                IRoomModifier modifier2 = new UnreachableTileCapture(roomData);
                IRoomModifier modifier3 = new TileFill(roomData);
                IRoomModifier modifier4 = new UnreachableTileDesctructor(roomData);

                generatedTiles.UnionWith(
                    modifier4.ModifyRoom(
                    modifier3.ModifyRoom(
                    modifier2.ModifyRoom(
                    modifier1.ModifyRoom(
                    generator5.GenerateRoom(
                    generator4.GenerateRoom(
                    generator3.GenerateRoom(
                    generator2.GenerateRoom(
                    generator1.GenerateRoom(null))))))))));
            }

            m_renderer.DrawTiles(generatedTiles);

            // Connect rooms with corridors
        }

        public void ClearLocation() => m_renderer.Clear();

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Vector2 upLeft = new Vector2(transform.position.x - m_xBorderSize / 2, transform.position.y + m_yBorderSize / 2);
            Vector2 upRight = new Vector2(transform.position.x + m_xBorderSize / 2, transform.position.y + m_yBorderSize / 2);
            Vector2 downRight = new Vector2(transform.position.x + m_xBorderSize / 2, transform.position.y - m_yBorderSize / 2);
            Vector2 downLeft = new Vector2(transform.position.x - m_xBorderSize / 2, transform.position.y - m_yBorderSize / 2);

            Gizmos.DrawLine(upLeft, upRight);
            Gizmos.DrawLine(upRight, downRight);
            Gizmos.DrawLine(downRight, downLeft);
            Gizmos.DrawLine(downLeft, upLeft);
        }
    }
}

