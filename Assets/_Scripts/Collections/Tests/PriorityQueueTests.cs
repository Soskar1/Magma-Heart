using NUnit.Framework;
using MagmaHeart.Collections;
using System;

namespace MagmaHeart.Tests
{
    public class PriorityQueueTests
    {
        [Test]
        public void PriorityQueue_EnqueueDequeueOneElement_Success()
        {
            PriorityQueue<int, int> queue = new PriorityQueue<int, int>();
            int valueToInsert = 10;
            queue.Enqueue(valueToInsert, 2);

            Assert.That(queue.Count, Is.EqualTo(1));

            int value = queue.Dequeue();
            Assert.That(queue.Count, Is.EqualTo(0));
            Assert.That(value, Is.EqualTo(valueToInsert));
        }

        [Test]
        [TestCase(1, 3, 2, 4)]
        [TestCase(5, 5, 2, 2)]
        [TestCase(1, 1, 2, 1)]
        [TestCase(2, 1, 1, 1)]
        public void PriorityQueue_EnqueueDequeueTwoDistinctElements_Success(int firstValue, int firstPriority, int secondValue, int secondPriority)
        {
            PriorityQueue<int, int> queue = new PriorityQueue<int, int>();
            queue.Enqueue(firstValue, firstPriority);
            queue.Enqueue(secondValue, secondPriority);

            Assert.That(queue.Count, Is.EqualTo(2));

            int value = queue.Dequeue();
            Assert.That(queue.Count, Is.EqualTo(1));

            if (firstPriority <= secondPriority)
            {
                Assert.That(value, Is.EqualTo(firstValue));
                value = queue.Dequeue();
                Assert.That(value, Is.EqualTo(secondValue));
            }
            else
            {
                Assert.That(value, Is.EqualTo(secondValue));
                value = queue.Dequeue();
                Assert.That(value, Is.EqualTo(firstValue));
            }
        }

        [Test]
        public void PriorityQueue_EnqueueDequeueSameElements_Success()
        {
            PriorityQueue<int, int> queue = new PriorityQueue<int, int>();
            queue.Enqueue(1, 1);
            queue.Enqueue(1, 2);

            Assert.That(queue.Count, Is.EqualTo(2));

            queue.Dequeue();
            queue.Dequeue();

            Assert.That(queue.Count, Is.EqualTo(0));
        }

        [Test]
        public void PriorityQueue_DequeueEmptyQueue_ShouldThrow()
        {
            PriorityQueue<int, int> queue = new PriorityQueue<int, int>();
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        [Test]
        public void PriorityQueue_RemoveWithNonDistinctPriorities_RemovesElementSuccessfully()
        {
            PriorityQueue<int, int> queue = new PriorityQueue<int, int>();

            queue.Enqueue(1, 1);
            queue.Enqueue(2, 1);
            queue.Enqueue(3, 1);

            queue.Remove(2);

            Assert.That(queue.Count, Is.EqualTo(2));
            
            int value = queue.Dequeue();
            Assert.That(value, Is.EqualTo(1));

            value = queue.Dequeue();
            Assert.That(value, Is.EqualTo(3));
        }

        [Test]
        public void PriorityQueue_RemoveWithDistinctPriorities_RemovesElementSuccessfully()
        {
            PriorityQueue<int, int> queue = new PriorityQueue<int, int>();

            queue.Enqueue(1, 1);
            queue.Enqueue(2, 4);
            queue.Enqueue(3, 7);

            queue.Remove(2);

            Assert.That(queue.Count, Is.EqualTo(2));

            int value = queue.Dequeue();
            Assert.That(value, Is.EqualTo(1));

            value = queue.Dequeue();
            Assert.That(value, Is.EqualTo(3));
        }

        [Test]
        public void PriorityQueue_UpdatePriority_Success()
        {
            PriorityQueue<int, int> queue = new PriorityQueue<int, int>();

            queue.Enqueue(6, 10);
            queue.Enqueue(7, 11);
            queue.Enqueue(7, 12);
            queue.UpdatePriority(7, 8);

            Assert.That(queue.Count, Is.EqualTo(3));

            int value = queue.Dequeue();
            Assert.That(value, Is.EqualTo(7));

            value = queue.Dequeue();
            Assert.That(value, Is.EqualTo(6));

            value = queue.Dequeue();
            Assert.That(value, Is.EqualTo(7));
        }

        [Test]
        public void PriorityQueue_GetEnumerator_PerfectlyIteratesThroughItems()
        {
            PriorityQueue<int, int> queue = new PriorityQueue<int, int>();

            queue.Enqueue(1, 1);
            queue.Enqueue(2, 0);
            queue.Enqueue(3, 2);
            queue.Enqueue(4, 4);
            queue.Enqueue(5, 4);
            queue.Enqueue(6, 3);

            int i = 0;
            int[] expectedSequence = { 2, 1, 3, 6, 4, 5};
            foreach (int item in queue)
            {
                Assert.That(item, Is.EqualTo(expectedSequence[i]));
                ++i;
            }
        }
    }
}
