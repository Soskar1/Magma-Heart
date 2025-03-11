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
            if (!m_nodes.Contains(node))
                m_nodes.Add(node);
        }

        public void AddEdge(in RoomData first, in RoomData second)
        {
            if (!m_nodes.Contains(first))
            {
                Debug.LogWarning($"[LocationGraph.AddEdge] Graph does not contain {first} room node. Adding it to graph");
                m_nodes.Add(first);
            }

            if (!m_nodes.Contains(second))
            {
                Debug.LogWarning($"[LocationGraph.AddEdge] Graph does not contain {second} room node. Adding it to graph");
                m_nodes.Add(second);
            }

            RoomConnectionEdge newEdge = new RoomConnectionEdge(first, second);

            if (!m_edges[first].Any(edge => edge.Equals(newEdge)))
            {
                m_edges[first].Add(newEdge);
                m_edges[second].Add(newEdge);
            }
        }

        public void RemoveEdge(in RoomData first, in RoomData second)
        {
            if (!m_nodes.Contains(first))
            {
                Debug.LogWarning($"[LocationGraph.RemoveEdge] Graph does not contain {first} room node. Returning");
                return;
            }

            if (!m_nodes.Contains(second))
            {
                Debug.LogWarning($"[LocationGraph.RemoveEdge] Graph does not contain {second} room node. Returning");
                return;
            }

            RoomConnectionEdge edgeToRemove = new RoomConnectionEdge(first, second);

            m_edges[first].RemoveAll(edge => edge.Equals(edgeToRemove));
            m_edges[second].RemoveAll(edge => edge.Equals(edgeToRemove));
        }
    }
}