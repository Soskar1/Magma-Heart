using NUnit.Framework;

namespace MagmaHeart.Collections.Tests
{
    internal class CircularListTests
    {
        [Test]
        public void CircularList_Constructor_CreatesEmptyList()
        {
            CircularList<int> list = new CircularList<int>();

            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void CircularList_Add_AddsElement()
        {
            CircularList<int> list = new CircularList<int>();
            
            list.Add(1);

            Assert.That(list.Count, Is.EqualTo(1));
        }

        [Test]
        public void CircularList_Remove_RemovesElement()
        {
            CircularList<int> list = new CircularList<int>();
            list.Add(1);

            list.Remove(1);

            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void CircularList_Clear_ClearsElements()
        {
            CircularList<int> list = new CircularList<int>() { 1, 2, 3, 4, 5 };

            list.Clear();

            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void CircularList_Contains_ReturnsTrue()
        {
            CircularList<int> list = new CircularList<int>() { 1, 2, 3, 4, 5 };

            bool result = list.Contains(4);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CircularList_Contains_ReturnsFalse()
        {
            CircularList<int> list = new CircularList<int> { 1, 2, 3 };

            bool result = list.Contains(4);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CircularList_Next_ReturnsNextElement()
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
        public void CircularList_Head_ReturnsCurrentElement()
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
    }
}
