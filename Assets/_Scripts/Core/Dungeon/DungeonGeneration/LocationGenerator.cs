using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using Random = System.Random;
using System;
using UnityEditor;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationGenerator : MonoBehaviour
    {
        private BinarySpacePartitioning m_spacePartitioning;
        private CorridorGenerator m_corridorGenerator;

        private Vector2Int m_locationSpaceSize;
        private Vector2Int m_roomBorderOffsets;

        private List<IRoomGenerator> m_generators = new List<IRoomGenerator>();
        private List<GameObject> m_debugElements = new List<GameObject>();

        [SerializeField] private TextAsset m_locationGeneratorXmlFile;
        [SerializeField] private bool m_useSeed;
        [SerializeField] private int m_seed;
        private Random m_random;

        private void Awake()
        {
            if (!m_useSeed)
                m_seed = Environment.TickCount;

            m_random = new Random(m_seed);

            LocationGeneratorDeserializer deserializer = new LocationGeneratorDeserializer(m_locationGeneratorXmlFile.name, m_random);
            LocationGeneratorData data = deserializer.Deserialize();
            m_generators = data.generators;
            m_locationSpaceSize = data.locationSpaceSize;
            m_roomBorderOffsets = data.roomBorderOffsets;
            m_spacePartitioning = data.partitioning;
            m_corridorGenerator = data.corridorGenerator;
        }

        public async Task<Location> GenerateLocation(Vector2Int position)
        {
            BoundsInt locationSpace = new BoundsInt(new Vector3Int(position.x - m_locationSpaceSize.x / 2, position.y - m_locationSpaceSize.y / 2, 0),
                new Vector3Int(m_locationSpaceSize.x, m_locationSpaceSize.y, 0));
            List<BoundsInt> spaces = new List<BoundsInt>();
            
            if (m_spacePartitioning != null)
                spaces = m_spacePartitioning.PerformBinarySpacePartitioning(locationSpace);
            else
                spaces.Add(locationSpace);

            HashSet<RoomTileData> roomTileDatas = new HashSet<RoomTileData>();

            if (m_debugElements.Count > 0)
                foreach (var obj in m_debugElements)
                    Destroy(obj);

            await Task.Run(() =>
            {
                foreach (BoundsInt space in spaces)
                {
                    RoomTileData roomTileData = GenerateRoom(space);
                    roomTileDatas.Add(roomTileData);
                }
            });

            // LocationGraph
            LocationGraphCreator graphCreator = new LocationGraphCreator(roomTileDatas);
            LocationGraph graph = graphCreator.CreateGraph();

            // MST
            MinimalSpanningTreeCreator mstCreator = new MinimalSpanningTreeCreator();
            RoomTileData startNode = roomTileDatas.ElementAt(m_random.Next(roomTileDatas.Count));
            LocationGraph mstGraph = mstCreator.ExtractMinimalSpanningTree(graph, startNode);

            HashSet<DungeonTile> corridorTiles = new HashSet<DungeonTile>();
            if (m_corridorGenerator != null)
            {
                await Task.Run(() => {
                    foreach (RoomConnectionEdge edge in mstGraph.Edges)
                    {
                        HashSet<DungeonTile> tiles = m_corridorGenerator.GenerateCorridor(edge.First, edge.Second);
                        corridorTiles.UnionWith(tiles);
                    }
                });
            }

            HashSet<Vector2Int> tilePositions = new HashSet<Vector2Int>();
            foreach (RoomTileData RoomTileData in roomTileDatas)
                tilePositions.UnionWith(RoomTileData.GetTilePositions());

            HashSet<Vector2Int> corridorTilePositions = corridorTiles.Select(tile => tile.Position).ToHashSet();
            tilePositions.UnionWith(corridorTilePositions);

            LocationWallGenerator wallGenerator = new LocationWallGenerator();
            HashSet<DungeonTile> wallTiles = wallGenerator.GenerateWalls(tilePositions);

            return new Location(roomTileDatas.ToList(), corridorTiles, wallTiles);
        }

        private RoomTileData GenerateRoom(in BoundsInt space)
        {
            RoomTileData RoomTileData = new RoomTileData(space, m_roomBorderOffsets);

            foreach (IRoomGenerator generator in m_generators)
                generator.GenerateRoom(RoomTileData);

            return RoomTileData;
        }
    }
}

