using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.Core.Dungeon
{
    public struct LocationGeneratorData
    {
        public Vector2Int locationSpaceSize;
        public Vector2Int roomBorderOffsets;
        public BinarySpacePartitioning partitioning;
        public List<IRoomGenerator> generators;
        public List<IRoomModifier> modifiers;
    }

    public class LocationGeneratorDeserializer
    {
        private XmlDocument m_document;
        private Random m_random;

        private const string LOCATION_GENERATOR_XPATH = "//LocationGenerator";

        public LocationGeneratorDeserializer(string pathToXml, Random random)
        {
            m_document = new XmlDocument();
            m_document.Load(pathToXml);
            m_random = random;
        }

        public LocationGeneratorData Deserialize()
        {
            LocationGeneratorData data = new LocationGeneratorData();

            XmlElement root = m_document.DocumentElement;
            if (root == null)
            {
                Debug.LogWarning("root element is null. Returning empty LocationGeneratorData");
                return new LocationGeneratorData();
            }

            XmlAttributeCollection locationSpaceElementAttributes = root.SelectSingleNode($"{LOCATION_GENERATOR_XPATH}/LocationSpace").Attributes;
            int xSize = Int32.Parse(locationSpaceElementAttributes["XSize"].Value);
            int ySize = Int32.Parse(locationSpaceElementAttributes["YSize"].Value);
            data.locationSpaceSize = new Vector2Int(xSize, ySize);

            XmlAttributeCollection roomSpaceElementAttributes = root.SelectSingleNode($"{LOCATION_GENERATOR_XPATH}/RoomSpace").Attributes;
            int xBorderOffset = Int32.Parse(roomSpaceElementAttributes["XBorderOffset"].Value);
            int yBorderOffset = Int32.Parse(roomSpaceElementAttributes["YBorderOffset"].Value);
            data.roomBorderOffsets = new Vector2Int(xBorderOffset, yBorderOffset);

            XmlAttributeCollection spacePartitioningAttributes = root.SelectSingleNode($"{LOCATION_GENERATOR_XPATH}/SpacePartitioning").Attributes;
            int maxPartitions = Int32.Parse(spacePartitioningAttributes["MaxPartitions"].Value);
            int roomXMinSize = Int32.Parse(spacePartitioningAttributes["RoomXMinSize"].Value);
            int roomYMinSize = Int32.Parse(spacePartitioningAttributes["RoomYMinSize"].Value);
            data.partitioning = new BinarySpacePartitioning(m_random, roomXMinSize, roomYMinSize, maxPartitions);

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
            data.generators = generators;

            List<IRoomModifier> modifiers = new List<IRoomModifier>();
            XmlNode roomModifiers = root.SelectSingleNode($"{LOCATION_GENERATOR_XPATH}/RoomModifiers");
            if (roomModifiers != null)
            {
                foreach (XmlNode roomModifier in roomModifiers.ChildNodes)
                {
                    IRoomModifier modifier = DeserializeRoomModifier(roomModifier);
                    modifiers.Add(modifier);
                }
            }
            data.modifiers = modifiers;

            return data;
        }

        private IRoomGenerator DeserializeRoomGenerator(XmlNode node)
        {
            switch (node.Name)
            {
                case "Boxed":
                    XmlAttributeCollection attributes = node.Attributes;
                    int xSize = Int32.Parse(attributes["XSize"].Value);
                    int ySize = Int32.Parse(attributes["YSize"].Value);
                    return new BoxedRoomGenerator(xSize, ySize);
                case "RandomWalk":
                    int iterations = Int32.Parse(node.Attributes["Iterations"].Value);
                    return new RandomWalkRoomGenerator(m_random, iterations);
                case "DiffusionLimitedAggregation":
                    int tilesToPlace = Int32.Parse(node.Attributes["TilesToPlace"].Value);
                    return new DiffusionLimitedAggregatoinRoomGenerator(m_random, tilesToPlace);
                default:
                    Debug.LogWarning($"{node.Name} not found. Returning null");
                    return null;
            }
        }

        private IRoomModifier DeserializeRoomModifier(XmlNode node)
        {
            switch (node.Name)
            {
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