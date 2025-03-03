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

        private readonly float m_horizontalPartitionThreshold;
        private readonly float m_verticalPartitionThreshold;
        private readonly float m_spaceSizeMultiplier;

        public BinarySpacePartitioning(in int xMinSize, in int yMinSize, in int maxPartitions, in float horizontalPartitionChance = 0.5f, in float verticalPartitionChance = 0.5f)
        {
            m_xMinSize = xMinSize;
            m_yMinSize = yMinSize;
            m_maxPartitions = maxPartitions;
            m_spaceSizeMultiplier = 2f;

            //if (horizontalPartitionChance + verticalPartitionChance != 1.0f)
            //{
            //    Debug.LogWarning($"Horizontal and vertical partition chance sum is not equal to 1 ({horizontalPartitionChance} + {verticalPartitionChance} = {horizontalPartitionChance + verticalPartitionChance}). Creating BinarySpacePartitioning object with default configuration");
            //    m_horizontalPartitionThreshold = 0.5f;
            //    m_verticalPartitionThreshold = 1.0f;
            //}
            //else
            //{
            //    m_horizontalPartitionThreshold = horizontalPartitionChance;
            //    m_verticalPartitionThreshold = 1.0f;
            //}
        }

        public List<BoundsInt> PerformBinarySpacePartitioning(in BoundsInt spaceToSplit)
        {
            Predicate<BoundsInt> CanPerformHorizontalPartition = (space) => space.size.x >= m_xMinSize * m_spaceSizeMultiplier + 1;
            Predicate<BoundsInt> CanPerformVerticalPartition = (space) => space.size.y >= m_yMinSize * m_spaceSizeMultiplier + 1;

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

                //float random = Random.Range(0.0f, 1.0f);
                float random = 0.6f;

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

                if (CanPerformHorizontalPartition(spaceTuple.space1) || CanPerformVerticalPartition(spaceTuple.space2))
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
            BoundsInt space1 = new BoundsInt(new Vector3Int(spaceToSplit.position.x, (ySplitPoint - spaceToSplit.yMin) / 2, spaceToSplit.position.z), new Vector3Int(spaceToSplit.size.x, ySplitPoint, spaceToSplit.size.z));
            BoundsInt space2 = new BoundsInt(new Vector3Int(spaceToSplit.position.x, (spaceToSplit.yMax - ySplitPoint) / 2, spaceToSplit.position.z), new Vector3Int(spaceToSplit.size.x, spaceToSplit.size.y - ySplitPoint, spaceToSplit.size.z));

            return (space1, space2);
        }

        private (BoundsInt, BoundsInt) SplitSpaceVertically(in BoundsInt spaceToSplit)
        {
            int xSplitPoint = Random.Range(spaceToSplit.xMin + m_xMinSize, spaceToSplit.xMax - m_xMinSize);

            int space1Offset = (int)spaceToSplit.center.x - (xSplitPoint - spaceToSplit.xMin) / 2;
            int space2Offset = (int)spaceToSplit.center.x - (spaceToSplit.xMax - xSplitPoint) / 2;

            BoundsInt space1 = new BoundsInt(new Vector3Int(spaceToSplit.position.x - space1Offset, spaceToSplit.position.y, spaceToSplit.position.z), new Vector3Int(xSplitPoint, spaceToSplit.size.y, spaceToSplit.size.z));
            BoundsInt space2 = new BoundsInt(new Vector3Int(spaceToSplit.position.x + space2Offset, spaceToSplit.position.y, spaceToSplit.position.z), new Vector3Int(spaceToSplit.size.x - xSplitPoint, spaceToSplit.size.y, spaceToSplit.size.z));

            return (space1, space2);
        }
    }
}