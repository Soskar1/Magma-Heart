using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomConnectionEdge
    {
        public float Cost { get; private set; }
        public RoomTileData First { get; private set; }
        public RoomTileData Second { get; private set; }

        public RoomConnectionEdge()
        {
            First = null;
            Second = null;
            Cost = Mathf.Infinity;
        }

        public RoomConnectionEdge(in RoomTileData first, in RoomTileData second)
        {
            First = first;
            Second = second;
            Cost = Vector2Int.Distance(first.WorldPosition, second.WorldPosition);
        }

        public override bool Equals(object obj)
        {
            RoomConnectionEdge edge = obj as RoomConnectionEdge;

            if (edge == null)
                return false;

            if (edge.First == First && edge.Second == Second)
                return true;

            if (edge.First == Second && edge.Second == First)
                return true;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + First.GetHashCode();
            hash = hash * 31 + Second.GetHashCode();
            return hash;
        }
    }

    public class LocationGraph
    {
        public List<RoomTileData> Nodes { get; private set; }
        public HashSet<RoomConnectionEdge> Edges { get; private set; }
        public Dictionary<RoomTileData, HashSet<RoomConnectionEdge>> EdgesFromRoom { get; private set; }
        public int NodeCount => Nodes.Count;

        public LocationGraph()
        {
            Nodes = new List<RoomTileData>();
            Edges = new HashSet<RoomConnectionEdge>();
            EdgesFromRoom = new Dictionary<RoomTileData, HashSet<RoomConnectionEdge>>();
        }

        public LocationGraph(in List<RoomTileData> nodes, in HashSet<RoomConnectionEdge> edges)
        {
            Nodes = nodes;
            Edges = edges;
            EdgesFromRoom = new Dictionary<RoomTileData, HashSet<RoomConnectionEdge>>();

            foreach (RoomTileData node in Nodes)
                EdgesFromRoom.Add(node, new HashSet<RoomConnectionEdge>());

            foreach (RoomConnectionEdge edge in Edges)
            {
                EdgesFromRoom[edge.First].Add(edge);
                EdgesFromRoom[edge.Second].Add(edge);
            }
        }

        public void TryAddNode(in RoomTileData node)
        {
            Nodes.Add(node);
            EdgesFromRoom.TryAdd(node, new HashSet<RoomConnectionEdge>());
        }

        public void TryAddEdge(in RoomConnectionEdge edge)
        {
            if (!Edges.Contains(edge))
            {
                TryAddNode(edge.First);
                TryAddNode(edge.Second);

                Edges.Add(edge);
                EdgesFromRoom[edge.First].Add(edge);
                EdgesFromRoom[edge.Second].Add(edge);
            }
        }
        public bool ContainsEdge(in RoomConnectionEdge edge) => Edges.Contains(edge);
    }
}