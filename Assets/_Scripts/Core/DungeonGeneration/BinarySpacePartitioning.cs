using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MagmaHeart.Core.Dungeon
{
    public class BinarySpacePartitioning
    {
        private readonly int m_xMinSize;
        private readonly int m_yMinSize;
        private readonly int m_maxPartitions;

        public BinarySpacePartitioning(in int xMinSize, in int yMinSize, in int maxPartitions)
        {
            m_xMinSize = xMinSize;
            m_yMinSize = yMinSize;
            m_maxPartitions = maxPartitions;
        }

        public List<BoundsInt> PerformBinarySpacePartitioning(in BoundsInt spaceToSplit)
        {
            Predicate<BoundsInt> CanPerformHorizontalPartition = (space) => space.size.y >= m_yMinSize * 2 + 1;
            Predicate<BoundsInt> CanPerformVerticalPartition = (space) => space.size.x >= m_xMinSize * 2 + 1;

            if (!CanPerformHorizontalPartition(spaceToSplit) && !CanPerformVerticalPartition(spaceToSplit))
            {
                Debug.LogWarning($"Can't perform horizontal and vertical partitioning for the provided space: {spaceToSplit}. Returning {spaceToSplit}.");
                return new List<BoundsInt>() { spaceToSplit };
            }

            List<BoundsInt> spaces = new List<BoundsInt>();
            Queue<BoundsInt> spaceQueue = new Queue<BoundsInt>();
            spaceQueue.Enqueue(spaceToSplit);
            int partitions = 0;

            while (partitions < m_maxPartitions && spaceQueue.Count > 0)
            {
                BoundsInt space = spaceQueue.Dequeue();
                (BoundsInt space1, BoundsInt space2) spaceTuple = (new BoundsInt(), new BoundsInt());

                float random = Random.Range(0.0f, 1.0f);

                if (random < 0.5f)
                {
                    // Horizontal partitioning
                    if (CanPerformHorizontalPartition(space))
                        (spaceTuple.space1, spaceTuple.space2) = SplitSpaceHorizontally(space);
                    else if (CanPerformVerticalPartition(space))
                        (spaceTuple.space1, spaceTuple.space2) = SplitSpaceVertically(space);
                }
                else
                {
                    // Vertical partitioning
                    if (CanPerformVerticalPartition(space))
                        (spaceTuple.space1, spaceTuple.space2) = SplitSpaceVertically(space);
                    else if (CanPerformHorizontalPartition(space))
                        (spaceTuple.space1, spaceTuple.space2) = SplitSpaceHorizontally(space);
                }

                if (CanPerformHorizontalPartition(spaceTuple.space1) || CanPerformVerticalPartition(spaceTuple.space1))
                    spaceQueue.Enqueue(spaceTuple.space1);
                else
                    spaces.Add(spaceTuple.space1);

                if (CanPerformHorizontalPartition(spaceTuple.space2) || CanPerformVerticalPartition(spaceTuple.space2))
                    spaceQueue.Enqueue(spaceTuple.space2);
                else
                    spaces.Add(spaceTuple.space2);

                ++partitions;
            }

            while (spaceQueue.Count > 0)
            {
                BoundsInt space = spaceQueue.Dequeue();
                spaces.Add(space);
            }

            return spaces;
        }

        private (BoundsInt, BoundsInt) SplitSpaceHorizontally(in BoundsInt spaceToSplit)
        {
            int ySplitPoint = Random.Range(spaceToSplit.yMin + m_yMinSize, spaceToSplit.yMax - m_yMinSize);

            BoundsInt space1 = new BoundsInt(spaceToSplit.position, new Vector3Int(spaceToSplit.size.x, ySplitPoint - spaceToSplit.yMin, spaceToSplit.size.z));
            BoundsInt space2 = new BoundsInt(new Vector3Int(spaceToSplit.position.x, space1.yMax, spaceToSplit.position.z), new Vector3Int(spaceToSplit.size.x, spaceToSplit.size.y - space1.size.y, spaceToSplit.size.z));
            
            return (space1, space2);
        }

        private (BoundsInt, BoundsInt) SplitSpaceVertically(in BoundsInt spaceToSplit)
        {
            int xSplitPoint = Random.Range(spaceToSplit.xMin + m_xMinSize, spaceToSplit.xMax - m_xMinSize);

            BoundsInt space1 = new BoundsInt(spaceToSplit.position, new Vector3Int(xSplitPoint - spaceToSplit.xMin, spaceToSplit.size.y, spaceToSplit.size.z));
            BoundsInt space2 = new BoundsInt(new Vector3Int(space1.xMax, spaceToSplit.position.y, spaceToSplit.position.z), new Vector3Int(spaceToSplit.size.x - space1.size.x, spaceToSplit.size.y, spaceToSplit.size.z));

            return (space1, space2);
        }
    }
}