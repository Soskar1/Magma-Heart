using NUnit.Framework;

namespace MagmaHeart.Collections.Tests
{
    public class ChainNodeTests
    {
        [Test]
        public void ChainNode_Next_ReturnsNextNode()
        {
            ChainNode<int> node1 = new ChainNode<int>(3);
            ChainNode<int> node2 = new ChainNode<int>(4);
            
            node1.Next = node2;

            Assert.That(node1.Value, Is.EqualTo(3));
            Assert.That(node1.Next.Value, Is.EqualTo(4));
            Assert.That(node2.Next, Is.Null);
        }

        [Test]
        public void ChainNode_CircularListConvert_ReturnsCyclicChainNode()
        {
            CircularList<int> list = new CircularList<int>() { 1, 2, 3 };

            ChainNode<int> chain = (ChainNode<int>)list;

            Assert.That(chain.Value, Is.EqualTo(1));
            Assert.That(chain.Next.Value, Is.EqualTo(2));
            Assert.That(chain.Next.Next.Value, Is.EqualTo(3));
            Assert.That(chain.Next.Next.Next.Value, Is.EqualTo(1));
        }

        [Test]
        public void ChainNode_CircularListConvert_ReturnsCyclicChainNodeFromTheHead()
        {
            CircularList<int> list = new CircularList<int>() { 1, 2, 3 };
            list.Next();

            ChainNode<int> chain = (ChainNode<int>)list;

            Assert.That(chain.Value, Is.EqualTo(2));
            Assert.That(chain.Next.Value, Is.EqualTo(3));
            Assert.That(chain.Next.Next.Value, Is.EqualTo(1));
            Assert.That(chain.Next.Next.Next.Value, Is.EqualTo(2));
        }
    }
}