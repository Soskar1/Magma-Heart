using UnityEngine;
using System.Collections.Generic;

namespace MagmaHeart.Core.Dungeon
{
    public class MinimalSpanningTreeCreator
    {
        public LocationGraph ExtractMinimalSpanningTree(in LocationGraph graph, in RoomTileData startNode)
        {
            LocationGraph mstGraph = new LocationGraph();
            mstGraph.TryAddNode(startNode);

            while (mstGraph.NodeCount < graph.NodeCount)
            {
                HashSet<RoomConnectionEdge> edges = new HashSet<RoomConnectionEdge>();

                foreach (RoomTileData roomTileData in mstGraph.Nodes)
                    edges.UnionWith(graph.EdgesFromRoom[roomTileData]);

                edges.ExceptWith(mstGraph.Edges);

                RoomConnectionEdge minEdge = new RoomConnectionEdge();
                foreach (RoomConnectionEdge edge in edges)
                    if (edge.Cost < minEdge.Cost && !mstGraph.ContainsEdge(edge) &&
                        ((!mstGraph.Nodes.Contains(edge.First) && mstGraph.Nodes.Contains(edge.Second)) ||
                        (!mstGraph.Nodes.Contains(edge.Second) && mstGraph.Nodes.Contains(edge.First))))
                        minEdge = edge;
                
                if (minEdge.Cost != Mathf.Infinity)
                    mstGraph.TryAddEdge(minEdge);
            }

            return mstGraph;
        }
    }
}