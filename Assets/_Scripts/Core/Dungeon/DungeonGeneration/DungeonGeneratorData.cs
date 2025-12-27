using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.Core.Dungeon
{
    public record DungeonGeneratorData(Vector2Int RoomSpaceSize, List<IRoomGenerator> Generators);

    public class DungeonGeneratorDataDeserializer
    {
        private readonly XmlDocument m_document;
        private Random m_random;

        private const string LOCATION_GENERATOR_XPATH = "//DungeonGenerator";

        public DungeonGeneratorDataDeserializer(TextAsset configFile, Random random)
        {
            m_document = new XmlDocument();
            m_document.Load(new StringReader(configFile.text));
            m_random = random;
        }

        public DungeonGeneratorData Deserialize()
        {
            XmlElement root = m_document.DocumentElement;
            if (root == null)
            {
                Debug.LogWarning("root element is null. Returning empty DungeonGeneratorData");
                return null;
            }

            XmlAttributeCollection roomSpaceElementAttributes = root.SelectSingleNode($"{LOCATION_GENERATOR_XPATH}/RoomSpace").Attributes;
            int xSize = Int32.Parse(roomSpaceElementAttributes["XSize"].Value);
            int ySize = Int32.Parse(roomSpaceElementAttributes["YSize"].Value);
            Vector2Int roomSpaceSize = new Vector2Int(xSize, ySize);

            List<IRoomGenerator> generators = new List<IRoomGenerator>();
            XmlNode roomGenerators = root.SelectSingleNode($"{LOCATION_GENERATOR_XPATH}/RoomGenerators");
            if (roomGenerators != null)
            {
                foreach (XmlNode roomGenerator in roomGenerators.ChildNodes)
                {
                    IRoomGenerator generator = DeserializeRoomGenerator(roomGenerator);
                    generators.Add(generator);
                }
            }

            return new DungeonGeneratorData(roomSpaceSize, generators);
        }

        private IRoomGenerator DeserializeRoomGenerator(XmlNode node)
        {
            switch (node.Name)
            {
                case "Boxed":
                    XmlAttributeCollection boxedRoomAttributes = node.Attributes;
                    int xSize = Int32.Parse(boxedRoomAttributes["XSize"].Value);
                    int ySize = Int32.Parse(boxedRoomAttributes["YSize"].Value);
                    return new BoxedRoomGenerator(xSize, ySize);
                case "RandomWalk":
                    int iterations = Int32.Parse(node.Attributes["Iterations"].Value);
                    return new RandomWalkRoomGenerator(m_random, iterations);
                case "DiffusionLimitedAggregation":
                    int tilesToPlace = Int32.Parse(node.Attributes["TilesToPlace"].Value);
                    return new DiffusionLimitedAggregatoinRoomGenerator(m_random, tilesToPlace);
                case "WallGenerator":
                    XmlAttributeCollection wallGeneratorAttributes = node.Attributes;
                    int maxWallLength = Int32.Parse(wallGeneratorAttributes["MaxWallLength"].Value);
                    int amountOfWalls = Int32.Parse(wallGeneratorAttributes["AmountOfWalls"].Value);
                    return new RoomWallGenerator(m_random, amountOfWalls, maxWallLength);
                case "TilePropagation":
                    int length = Int32.Parse(node.Attributes["Length"].Value);
                    return new TilePropagation(length);
                case "UnreachableTileCapture":
                    return new UnreachableTileCapture();
                case "TileFill":
                    return new TileFill();
                case "UnreachableTileDestructor":
                    return new UnreachableTileDestructor();
                default:
                    Debug.LogWarning($"{node.Name} not found. Returning null");
                    return null;
            }
        }
    }
}