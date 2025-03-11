using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomConnectionEdge
    {
        public float Cost { get; private set; }
        public RoomData First { get; private set; }
        public RoomData Second { get; private set; }

        public RoomConnectionEdge(in RoomData first, in RoomData second)
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
        public List<RoomData> Nodes { get; private set; }
        public Dictionary<RoomData, List<RoomConnectionEdge>> Edges { get; private set; }

        public LocationGraph(in List<RoomData> nodes, in Dictionary<RoomData, List<RoomConnectionEdge>> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public void AddNode(in RoomData node)
        {
            if (!Nodes.Contains(node))
                Nodes.Add(node);
        }

        public void AddEdge(in RoomData first, in RoomData second)
        {
            if (!Nodes.Contains(first))
            {
                Debug.LogWarning($"[LocationGraph.AddEdge] Graph does not contain {first} room node. Adding it to graph");
                Nodes.Add(first);
            }

            if (!Nodes.Contains(second))
            {
                Debug.LogWarning($"[LocationGraph.AddEdge] Graph does not contain {second} room node. Adding it to graph");
                Nodes.Add(second);
            }

            RoomConnectionEdge newEdge = new RoomConnectionEdge(first, second);

            if (!Edges[first].Any(edge => edge.Equals(newEdge)))
            {
                Edges[first].Add(newEdge);
                Edges[second].Add(newEdge);
            }
        }

        public void RemoveEdge(in RoomData first, in RoomData second)
        {
            if (!Nodes.Contains(first))
            {
                Debug.LogWarning($"[LocationGraph.RemoveEdge] Graph does not contain {first} room node. Returning");
                return;
            }

            if (!Nodes.Contains(second))
            {
                Debug.LogWarning($"[LocationGraph.RemoveEdge] Graph does not contain {second} room node. Returning");
                return;
            }

            RoomConnectionEdge edgeToRemove = new RoomConnectionEdge(first, second);

            Edges[first].RemoveAll(edge => edge.Equals(edgeToRemove));
            Edges[second].RemoveAll(edge => edge.Equals(edgeToRemove));
        }
    }
}