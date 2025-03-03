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
        [SerializeField] private GameObject m_spaceVizualizer;
        [SerializeField] private int m_xMinSize;
        [SerializeField] private int m_yMinSize;
        [SerializeField] private int m_maxPartitions;
        private List<GameObject> createdObjects = new List<GameObject>();

        private void Awake()
        {
            m_renderer = new LocationRenderer(m_tilemap, m_floorTile, m_wallTile);
        }

        public void GenerateLocation() => GenerateLocation(Vector2Int.zero);

        public void GenerateLocation(in Vector2Int position)
        {
            // Generate spaces for rooms
            foreach (var obj in createdObjects)
                Destroy(obj);

            BinarySpacePartitioning spacePartitioning = new BinarySpacePartitioning(m_xMinSize, m_yMinSize, m_maxPartitions);
            BoundsInt locationSpace = new BoundsInt(new Vector3Int(position.x, position.y, 0), new Vector3Int(m_xBorderSize, m_yBorderSize, 0));
            List<BoundsInt> spaces = spacePartitioning.PerformBinarySpacePartitioning(locationSpace);

            foreach (BoundsInt space in spaces)
            {
                GameObject testObject = Instantiate(m_spaceVizualizer, space.position, Quaternion.identity);
                testObject.transform.localScale = space.size;
                testObject.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
                createdObjects.Add(testObject);
            }

            //RoomData roomData = new RoomData(position, m_xBorderSize, m_yBorderSize);
            //IRoomGenerator generator1 = new BoxedRoomGenerator(roomData, m_xSize, m_ySize); 
            //IRoomGenerator generator2 = new DiffusionLimitedAggregatoinRoomGenerator(roomData, m_tilesToPlace);
            //IRoomGenerator generator3 = new RandomWalkRoomGenerator(roomData, m_randomWalkIterations);
            //IRoomGenerator generator4 = new DiffusionLimitedAggregatoinRoomGenerator(roomData, m_tilesToPlace);
            //IRoomGenerator generator5 = new RandomWalkRoomGenerator(roomData, m_randomWalkIterations);
            //IRoomModifier modifier1 = new UnreachableTileCapture(roomData);
            //IRoomModifier modifier2 = new UnreachableTileDesctructor(roomData);
            //IRoomModifier modifier3 = new TileFill(roomData);
            //IRoomModifier modifier4 = new TilePropagation(roomData, m_propagationLength);
            //HashSet<Vector2Int> generatedTiles = modifier3.ModifyRoom(
            //    modifier2.ModifyRoom(
            //    modifier4.ModifyRoom(
            //    modifier1.ModifyRoom(
            //    generator5.GenerateRoom(
            //    generator4.GenerateRoom(
            //    generator3.GenerateRoom(
            //    generator2.GenerateRoom(
            //    generator1.GenerateRoom(null)))))))));

            //m_renderer.DrawTiles(generatedTiles);

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

