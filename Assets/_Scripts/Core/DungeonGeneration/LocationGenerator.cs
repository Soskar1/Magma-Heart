using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;
using System.Linq;

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
        [SerializeField] private bool m_bspDebug;
        [SerializeField] private int m_xMinSize;
        [SerializeField] private int m_yMinSize;
        [SerializeField] private int m_maxPartitions;
        [SerializeField] private GameObject m_spaceVizualizer;

        [Header("Room space")]
        [SerializeField] private int m_xBorderOffset;
        [SerializeField] private int m_yBorderOffset;

        [Header("Location graph")]
        [SerializeField] private bool m_locationGraphDebug;
        [SerializeField] private bool m_mstTreeDebug;
        [SerializeField] private GameObject m_roomNodeDebug;
        [SerializeField] private GameObject m_roomEdgeDebug;
        
        [Header("Corridor generator")]
        [SerializeField] private int m_corridorSize;
        [SerializeField] private int m_pointsPerSegment;
        [SerializeField] private List<GameObject> m_curvePoints;
        [SerializeField] private GameObject m_curveDebug;

        private List<GameObject> m_debugElements = new List<GameObject>();
        private List<IRoomGenerator> m_generators = new List<IRoomGenerator>();
        private List<IRoomModifier> m_modifiers = new List<IRoomModifier>();

        private void Awake()
        {
            m_renderer = new LocationRenderer(m_tilemap, m_floorTile, m_wallTile);

            IRoomGenerator generator1 = new BoxedRoomGenerator(m_xSize, m_ySize); 
            IRoomGenerator generator2 = new RandomWalkRoomGenerator(m_randomWalkIterations);
            IRoomGenerator generator3 = new DiffusionLimitedAggregatoinRoomGenerator(m_tilesToPlace);
            IRoomModifier modifier1 = new TilePropagation(m_propagationLength);
            IRoomModifier modifier2 = new UnreachableTileCapture();
            IRoomModifier modifier3 = new TileFill();
            IRoomModifier modifier4 = new UnreachableTileDesctructor();

            m_generators.Add(generator1);
            m_generators.Add(generator2);
            m_generators.Add(generator3);
            m_generators.Add(generator2);
            m_generators.Add(generator3);

            m_modifiers.Add(modifier1);
            m_modifiers.Add(modifier2);
            m_modifiers.Add(modifier3);
            m_modifiers.Add(modifier4);
        }

        public void GenerateLocation() => GenerateLocation(Vector2Int.zero);

        public async void GenerateLocation(Vector2Int position)
        {
            BinarySpacePartitioning spacePartitioning = new BinarySpacePartitioning(m_xMinSize, m_yMinSize, m_maxPartitions);
            BoundsInt locationSpace = new BoundsInt(new Vector3Int(position.x - m_xBorderSize / 2, position.y - m_yBorderSize / 2, 0), new Vector3Int(m_xBorderSize, m_yBorderSize, 0));
            List<BoundsInt> spaces = spacePartitioning.PerformBinarySpacePartitioning(locationSpace);
            
            HashSet<RoomData> roomDatas = new HashSet<RoomData>();
            HashSet<Vector2Int> generatedTiles = new HashSet<Vector2Int>();

            if (m_debugElements.Count > 0)
                foreach (var obj in m_debugElements)
                    Destroy(obj);

            if (m_bspDebug)
            {
                foreach (BoundsInt space in spaces)
                {
                    GameObject testObject = Instantiate(m_spaceVizualizer, space.center, Quaternion.identity);
                    testObject.transform.localScale = space.size;
                    testObject.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
                    m_debugElements.Add(testObject);
                }
            }

            await Task.Run(() =>
            {
                foreach (BoundsInt space in spaces)
                {
                    RoomData roomData = GenerateRoom(space);
                    roomDatas.Add(roomData);
                    generatedTiles.UnionWith(roomData.GetTilesCopy());
                }
            });

            // LocationGraph
            LocationGraphCreator graphCreator = new LocationGraphCreator(roomDatas);
            LocationGraph graph = graphCreator.CreateGraph();

            if (m_locationGraphDebug)
                GraphDebug(graph);

            // MST
            MinimalSpanningTreeCreator mstCreator = new MinimalSpanningTreeCreator();
            RoomData startNode = roomDatas.ElementAt(Random.Range(0, roomDatas.Count));
            LocationGraph mstGraph = mstCreator.ExtractMinimalSpanningTree(graph, startNode);

            if (m_mstTreeDebug)
                GraphDebug(mstGraph);

            await Task.Run(() => {
                CorridorGenerator corridorGenerator = new CorridorGenerator(m_corridorSize);
                foreach (RoomConnectionEdge edge in mstGraph.Edges)
                {
                    HashSet<Vector2Int> corridorTiles = corridorGenerator.GenerateCorridor(edge.First, edge.Second);
                    generatedTiles.UnionWith(corridorTiles);
                }
            });

            List<Vector2> curve = new List<Vector2>();
            foreach (var obj in m_curvePoints)
                curve.Add(obj.transform.position);

            CurveGenerator curveGenerator = new CurveGenerator();
            curve = curveGenerator.GenerateSmoothCurve(curve, m_pointsPerSegment);

            foreach (var v in curve)
            {
                var debug = Instantiate(m_curveDebug, v, Quaternion.identity);
                m_debugElements.Add(debug);
            }

            StartCoroutine(m_renderer.DrawTiles(generatedTiles));
        }

        private void GraphDebug(in LocationGraph graph)
        {
            foreach (RoomData room in graph.Nodes)
            {
                GameObject nodeDebugInstance = Instantiate(m_roomNodeDebug, room.RoomSpace.center, Quaternion.identity);
                m_debugElements.Add(nodeDebugInstance);
            }

            foreach (RoomConnectionEdge edge in graph.Edges)
            {
                GameObject edgeDebugInstance = Instantiate(m_roomEdgeDebug, Vector3.zero, Quaternion.identity);
                m_debugElements.Add(edgeDebugInstance);

                LineRenderer[] edgeRenderers = edgeDebugInstance.GetComponentsInChildren<LineRenderer>();
                foreach (LineRenderer edgeRenderer in edgeRenderers)
                    edgeRenderer.SetPositions(new Vector3[2] { edge.First.RoomSpace.center, edge.Second.RoomSpace.center });
            }
        }

        private RoomData GenerateRoom(in BoundsInt space)
        {
            RoomData roomData = new RoomData(space, m_xBorderOffset, m_yBorderOffset);

            foreach (IRoomGenerator generator in m_generators)
                generator.GenerateRoom(roomData);

            foreach (IRoomModifier modifier in m_modifiers)
                modifier.ModifyRoom(roomData);

            return roomData;
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

