using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Collections.Tests
{
    internal class CircularListTests
    {
        [Test]
        public void Constructor_CreatesEmptyList()
        {
            CircularList<int> list = new CircularList<int>();

            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void Add_AddsElement()
        {
            CircularList<int> list = new CircularList<int>();

            list.Add(1);

            Assert.That(list.Count, Is.EqualTo(1));
        }

        [Test]
        public void Remove_RemovesElement()
        {
            CircularList<int> list = new CircularList<int>();
            list.Add(1);

            list.Remove(1);

            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void Clear_ClearsElements()
        {
            CircularList<int> list = new CircularList<int>() { 1, 2, 3, 4, 5 };

            list.Clear();

            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void Contains_ReturnsTrue()
        {
            CircularList<int> list = new CircularList<int>() { 1, 2, 3, 4, 5 };

            bool result = list.Contains(4);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Contains_ReturnsFalse()
        {
            CircularList<int> list = new CircularList<int> { 1, 2, 3 };

            bool result = list.Contains(4);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Next_ReturnsNextElement()
        {
            CircularList<int> list = new CircularList<int> { 1, 2, 3 };

            int[] results = new int[4];
            for (int i = 0; i < results.Length; ++i)
                results[i] = list.Next();

            Assert.That(results[0], Is.EqualTo(2));
            Assert.That(results[1], Is.EqualTo(3));
            Assert.That(results[2], Is.EqualTo(1));
            Assert.That(results[3], Is.EqualTo(2));
        }

        [Test]
        public void Head_ReturnsCurrentElement()
        {
            CircularList<int> list = new CircularList<int> { 1, 2, 3 };

            int[] results = new int[4];
            for (int i = 0; i < results.Length; ++i)
            {
                results[i] = list.Head;
                list.Next();
            }

            Assert.That(results[0], Is.EqualTo(1));
            Assert.That(results[1], Is.EqualTo(2));
            Assert.That(results[2], Is.EqualTo(3));
            Assert.That(results[3], Is.EqualTo(1));
        }

        [Test]
        public void CircluarList_AddRange_AddsRangeOfElements()
        {
            CircularList<int> list = new CircularList<int>();

            list.AddRange(new int[] { 1, 2, 3 });

            Assert.That(list.Count, Is.EqualTo(3));
        }

        [Test]
        public void NextTo_ReturnsNextElement()
        {
            CircularList<int> list = new CircularList<int>() { 1, 2, 3, 4, 5 };

            int value = list.NextTo(4);

            Assert.That(value, Is.EqualTo(5));
        }

        [Test]
        public void NextTo_ReturnsHeadElement()
        {
            CircularList<int> list = new CircularList<int>() { 1, 2, 3, 4, 5 };

            int value = list.NextTo(5);

            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void NextTo_ReturnsDuplicateElement()
        {
            CircularList<int> list = new CircularList<int>() { 1, 2, 3, 4, 5, 5 };

            int value = list.NextTo(5);

            Assert.That(value, Is.EqualTo(5));
        }

        [Test]
        public void IEnumerable_FromStart_WorksAsExpected()
        {
            CircularList<int> circularList = new CircularList<int>() { 1, 2, 3, 4, 5 };

            List<int> list = circularList.ToList();

            Assert.That(list, Is.EqualTo(new[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void IEnumerable_AfterNext_WorksAsExpected()
        {
            CircularList<int> circularList = new CircularList<int>() { 1, 2, 3, 4, 5 };
            circularList.Next();
            circularList.Next();

            List<int> list = circularList.ToList();

            Assert.That(list, Is.EqualTo(new[] { 3, 4, 5, 1, 2 }));
        }
    }
}
