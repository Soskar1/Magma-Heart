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

            List<RoomTileData> roomTileDatas = new List<RoomTileData>();

            await Task.Run(() =>
            {
                foreach (BoundsInt space in spaces)
                {
                    RoomTileData roomTileData = GenerateRoom(space);
                    roomTileDatas.Add(roomTileData);
                }
            });

            LocationGraph graph = CreateGraph(roomTileDatas);

            List<Corridor> corridors = new List<Corridor>();
            if (m_corridorGenerator != null)
            {
                await Task.Run(() => {
                    foreach (RoomConnectionEdge edge in graph.Edges)
                    {
                        Corridor corridor = m_corridorGenerator.GenerateCorridor(edge.First, edge.Second);
                        corridors.Add(corridor);
                    }
                });
            }
            
            AddWalls(roomTileDatas, corridors);

            return new Location(roomTileDatas, corridors, graph);
        }

        private void AddWalls(List<RoomTileData> roomTileDatas, List<Corridor> corridors)
        {
            HashSet<DungeonTile> allTiles = new HashSet<DungeonTile>();
            foreach (RoomTileData roomTileData in roomTileDatas)
                allTiles.UnionWith(roomTileData.GetTiles());

            foreach (Corridor corridor in corridors)
                allTiles.UnionWith(corridor.TileData.GetTiles());

            HashSet<DungeonTile> outline = TileOutline.GetOutline(allTiles);

            foreach (DungeonTile tile in outline)
                tile.Type = TileType.Wall;
        }

        private RoomTileData GenerateRoom(in BoundsInt space)
        {
            RoomTileData roomTileData = new RoomTileData(space, m_roomBorderOffsets);

            foreach (IRoomGenerator generator in m_generators)
                generator.GenerateRoom(roomTileData);

            return roomTileData;
        }

        private LocationGraph CreateGraph(in List<RoomTileData> rooms)
        {
            LocationGraphCreator graphCreator = new LocationGraphCreator(rooms);
            LocationGraph graph = graphCreator.CreateGraph();

            MinimalSpanningTreeCreator mstCreator = new MinimalSpanningTreeCreator();
            RoomTileData startNode = rooms[m_random.Next(rooms.Count)];
            return mstCreator.ExtractMinimalSpanningTree(graph, startNode);
        }
    }
}

