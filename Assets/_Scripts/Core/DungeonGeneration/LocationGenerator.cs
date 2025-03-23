using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;
using System.Linq;
using Random = System.Random;
using System;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationGenerator : MonoBehaviour
    {
        private LocationRenderer m_renderer;
        private BinarySpacePartitioning m_spacePartitioning;
        private CorridorGenerator m_corridorGenerator;

        private Vector2Int m_locationSpaceSize;
        private Vector2Int m_roomBorderOffsets;

        private List<IRoomGenerator> m_generators = new List<IRoomGenerator>();
        private List<IRoomModifier> m_modifiers = new List<IRoomModifier>();
        private List<GameObject> m_debugElements = new List<GameObject>();

        [SerializeField] private TextAsset m_locationGeneratorXmlFile;

        [SerializeField] private Tilemap m_tilemap;
        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private TileBase m_wallTile;

        [SerializeField] private bool m_useSeed;
        [SerializeField] private int m_seed;
        private Random m_random;

        [Header("BinarySpacePartitioning")]
        [SerializeField] private bool m_bspDebug;
        [SerializeField] private GameObject m_spaceVizualizer;

        [Header("Location graph")]
        [SerializeField] private bool m_locationGraphDebug;
        [SerializeField] private bool m_mstTreeDebug;
        [SerializeField] private GameObject m_roomNodeDebug;
        [SerializeField] private GameObject m_roomEdgeDebug;

        private void Awake()
        {
            if (!m_useSeed)
                m_seed = Environment.TickCount;

            m_random = new Random(m_seed);

            m_renderer = new LocationRenderer(m_tilemap, m_floorTile, m_wallTile);

            LocationGeneratorDeserializer deserializer = new LocationGeneratorDeserializer(m_locationGeneratorXmlFile.name, m_random);
            LocationGeneratorData data = deserializer.Deserialize();
            m_generators = data.generators;
            m_modifiers = data.modifiers;
            m_locationSpaceSize = data.locationSpaceSize;
            m_roomBorderOffsets = data.roomBorderOffsets;
            m_spacePartitioning = data.partitioning;
            m_corridorGenerator = data.corridorGenerator;
        }

        public void GenerateLocation() => GenerateLocation(Vector2Int.zero);

        public async void GenerateLocation(Vector2Int position)
        {
            BoundsInt locationSpace = new BoundsInt(new Vector3Int(position.x - m_locationSpaceSize.x / 2, position.y - m_locationSpaceSize.y / 2, 0),
                new Vector3Int(m_locationSpaceSize.x, m_locationSpaceSize.y, 0));
            List<BoundsInt> spaces = m_spacePartitioning.PerformBinarySpacePartitioning(locationSpace);
            
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
                    testObject.GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV();
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
            RoomData startNode = roomDatas.ElementAt(m_random.Next(roomDatas.Count));
            LocationGraph mstGraph = mstCreator.ExtractMinimalSpanningTree(graph, startNode);

            if (m_mstTreeDebug)
                GraphDebug(mstGraph);

           await Task.Run(() => {
                foreach (RoomConnectionEdge edge in mstGraph.Edges)
                {
                    HashSet<Vector2Int> corridorTiles = m_corridorGenerator.GenerateCorridor(edge.First, edge.Second);
                    generatedTiles.UnionWith(corridorTiles);
                }
           });

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
            RoomData roomData = new RoomData(space, m_roomBorderOffsets);

            foreach (IRoomGenerator generator in m_generators)
                generator.GenerateRoom(roomData);

            foreach (IRoomModifier modifier in m_modifiers)
                modifier.ModifyRoom(roomData);

            return roomData;
        }

        public void ClearLocation() => m_renderer.Clear();
    }
}

